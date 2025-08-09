using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class TankController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 12f;
    [SerializeField] private float maxReverseSpeed = 8f;
    [SerializeField] private float brakingForce = 4000f;
    [SerializeField] private float trackForce = 15000f;
    [SerializeField] private float steeringForce = 10000f;
    [SerializeField] private float stabilityForce = 10000f;
    
    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int pointsPerTrack = 8;
    
    [Header("Input Settings")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference turnAction;

    private Rigidbody rb;
    private float moveInput;
    private float turnInput;
    private List<TankTrack> tracks = new List<TankTrack>();
    private bool isGrounded;
    private float stabilizationFactor = 0.5f;

    private class TankTrack
    {
        public Transform trackTransform;
        public Vector3[] localPoints;
        public bool isRightSide;
        public Vector3 localLongAxis; // Главная ось гусеницы
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 5000f;
        rb.centerOfMass = Vector3.down * 0.5f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.linearDamping = 1f;
        rb.angularDamping = 5f;
        
        FindAndInitializeTracks();
    }

    private void FindAndInitializeTracks()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Track"))
            {
                CreateTrack(child);
            }
        }
        
        if (tracks.Count == 0)
        {
            Debug.LogError("No tracks found! Add child objects with 'Track' tag.");
        }
        else
        {
            Debug.Log($"Initialized {tracks.Count} tracks");
        }
    }

    private void CreateTrack(Transform trackTransform)
    {
        TankTrack newTrack = new TankTrack
        {
            trackTransform = trackTransform,
            isRightSide = IsRightSide(trackTransform)
        };

        CalculateTrackPoints(newTrack);
        tracks.Add(newTrack);
    }

    private bool IsRightSide(Transform trackTransform)
    {
        Vector3 localPos = transform.InverseTransformPoint(trackTransform.position);
        return localPos.x > 0;
    }

    private void CalculateTrackPoints(TankTrack track)
    {
        BoxCollider collider = track.trackTransform.GetComponent<BoxCollider>();
        if (!collider)
        {
            Debug.LogError($"BoxCollider not found on track: {track.trackTransform.name}");
            track.localPoints = new Vector3[0];
            return;
        }
        
        // Определяем главную ось гусеницы
        Vector3 size = collider.size;
        if (size.x >= size.y && size.x >= size.z)
        {
            track.localLongAxis = Vector3.right;
        }
        else if (size.y >= size.x && size.y >= size.z)
        {
            track.localLongAxis = Vector3.up;
        }
        else
        {
            track.localLongAxis = Vector3.forward;
        }

        // Генерация точек вдоль главной оси
        Vector3 startPoint = -track.localLongAxis * size.magnitude * 0.5f;
        Vector3 endPoint = track.localLongAxis * size.magnitude * 0.5f;
        
        track.localPoints = new Vector3[pointsPerTrack];
        for (int i = 0; i < pointsPerTrack; i++)
        {
            float t = i / (float)(pointsPerTrack - 1);
            Vector3 localPoint = Vector3.Lerp(startPoint, endPoint, t);
            track.localPoints[i] = localPoint;
        }
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        turnAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        turnAction.action.Disable();
    }

    private void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>().y;
        turnInput = turnAction.action.ReadValue<Vector2>().x;
    }

    private void FixedUpdate()
    {
        CheckGroundStatus();
        ApplyMovementForces();
        ApplyStabilization();
        ApplySpeedLimits();
        
        if (Mathf.Abs(moveInput) < 0.1f && Mathf.Abs(turnInput) < 0.1f)
        {
            ApplyBraking();
        }
    }

    private void CheckGroundStatus()
    {
        isGrounded = false;
        int groundedPoints = 0;

        foreach (TankTrack track in tracks)
        {
            foreach (Vector3 localPoint in track.localPoints)
            {
                Vector3 worldPoint = track.trackTransform.TransformPoint(localPoint);
                if (Physics.CheckSphere(worldPoint, groundCheckRadius, groundLayer))
                {
                    groundedPoints++;
                }
            }
        }

        isGrounded = groundedPoints > (tracks.Count * pointsPerTrack * 0.25f);
    }

    private void ApplyMovementForces()
    {
        if (!isGrounded) return;

        // Вычисляем направления движения корпуса
        Vector3 tankForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 tankRight = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        foreach (TankTrack track in tracks)
        {
            float forwardForceVal = trackForce * moveInput;
            float steeringMultiplier = track.isRightSide ? -1f : 1f;
            float steering = steeringForce * turnInput * steeringMultiplier;

            foreach (Vector3 localPoint in track.localPoints)
            {
                Vector3 worldPoint = track.trackTransform.TransformPoint(localPoint);
                
                if (Physics.CheckSphere(worldPoint, groundCheckRadius, groundLayer))
                {
                    // Основное движение вперед/назад
                    if (Mathf.Abs(moveInput) > 0.1f)
                    {
                        rb.AddForceAtPosition(tankForward * forwardForceVal, worldPoint);
                        Debug.DrawRay(worldPoint, tankForward * forwardForceVal * 0.001f, Color.green);
                    }
                    
                    // Поворот
                    if (Mathf.Abs(turnInput) > 0.1f)
                    {
                        Vector3 steeringDirection = tankRight * steeringMultiplier;
                        rb.AddForceAtPosition(steeringDirection * steering, worldPoint);
                        Debug.DrawRay(worldPoint, steeringDirection * steering * 0.001f, Color.blue);
                    }
                }
            }
        }
    }

    private void ApplyStabilization()
    {
        foreach (TankTrack track in tracks)
        {
            foreach (Vector3 localPoint in track.localPoints)
            {
                Vector3 worldPoint = track.trackTransform.TransformPoint(localPoint);
                
                // Вертикальная стабилизация
                RaycastHit hit;
                if (Physics.SphereCast(worldPoint + Vector3.up * 0.3f, groundCheckRadius, Vector3.down, out hit, 
                    groundCheckDistance + 0.3f, groundLayer))
                {
                    float distanceFactor = 1 - (hit.distance / groundCheckDistance);
                    Vector3 stabilizationForce = Vector3.up * distanceFactor * stabilityForce;
                    rb.AddForceAtPosition(stabilizationForce, worldPoint);
                }
                
                // Стабилизация крена
                Vector3 localPos = transform.InverseTransformPoint(worldPoint);
                float tiltFactor = Mathf.Abs(localPos.x) * stabilizationFactor;
                Vector3 antiRollForce = Vector3.up * tiltFactor * stabilityForce * 0.3f;
                rb.AddForceAtPosition(antiRollForce, worldPoint);
            }
        }
    }

    private void ApplyBraking()
    {
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 brakingDirection = -horizontalVelocity.normalized;
        float brakingMagnitude = Mathf.Min(brakingForce * Time.fixedDeltaTime, horizontalVelocity.magnitude);
        
        rb.AddForce(brakingDirection * brakingMagnitude, ForceMode.VelocityChange);
    }

    private void ApplySpeedLimits()
    {
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 forwardDirection = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        
        float currentSpeed = Vector3.Dot(horizontalVelocity, forwardDirection);
        
        if (currentSpeed > maxSpeed)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x - forwardDirection.x * (currentSpeed - maxSpeed),
                rb.linearVelocity.y,
                rb.linearVelocity.z - forwardDirection.z * (currentSpeed - maxSpeed)
            );
        }
        else if (currentSpeed < -maxReverseSpeed)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x - forwardDirection.x * (currentSpeed + maxReverseSpeed),
                rb.linearVelocity.y,
                rb.linearVelocity.z - forwardDirection.z * (currentSpeed + maxReverseSpeed)
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        
        foreach (TankTrack track in tracks)
        {
            Gizmos.color = track.isRightSide ? Color.red : Color.blue;
            
            foreach (Vector3 localPoint in track.localPoints)
            {
                Vector3 worldPoint = track.trackTransform.TransformPoint(localPoint);
                Gizmos.DrawSphere(worldPoint, groundCheckRadius);
                Gizmos.DrawLine(worldPoint, worldPoint + Vector3.down * groundCheckDistance);
            }
        }
    }
}