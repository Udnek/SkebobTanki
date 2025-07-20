using UnityEngine;
using UnityEngine.Serialization;

public class TankMovementReworked : MonoBehaviour
{
    [SerializeField] private float tankPullingForce = 20000f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float maxReverseSpeed = 8f;
    [SerializeField] private float speedLimiterFactor = 0.95f;
    [SerializeField] private float turningWhileMovingForceMultiplier = 0.5f;
    
    [SerializeField] private float steeringFactor = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float forceApplicationOffset = 1.5f;

    [SerializeField] private Rigidbody tankRigidbody;
    [SerializeField] private BoxCollider tankRightTrackTriggerCollider;
    [SerializeField] private BoxCollider tankLeftTrackTriggerCollider;
    
    public float movementDirection = -1;

    public Vector3 forceLeftTrackPositionLocal;
    public Vector3 forceRightTrackPositionLocal;

    private Vector3 forceLeftTrackPositionWorld;
    private Vector3 forceRightTrackPositionWorld;

    private float tankRightTrackPullingForce;
    private float tankLeftTrackPullingForce;
    
    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    private bool isTurningRight = false;
    private bool isTurningLeft = false;

    private bool isRightTrackGrounded;
    private bool isLeftTrackGrounded;
    
    private float forwardSpeed;

    private void Start()
    {
        forceLeftTrackPositionLocal = new Vector3(forceApplicationOffset, 0, 0);
        forceRightTrackPositionLocal = new Vector3(-forceApplicationOffset, 0, 0);
        
        tankRightTrackPullingForce = 0.5f * tankPullingForce;
        tankLeftTrackPullingForce = 0.5f * tankPullingForce;
        
        if (groundLayer.value == 0)
            groundLayer = LayerMask.GetMask("Default");
            
        tankRigidbody.centerOfMass = Vector3.zero;
        tankRigidbody.angularDamping = 5f;
    }
    
    private void FixedUpdate()
    {
        forceLeftTrackPositionWorld = transform.TransformPoint(forceLeftTrackPositionLocal);
        forceRightTrackPositionWorld = transform.TransformPoint(forceRightTrackPositionLocal);

        isRightTrackGrounded = CheckTrackGrounded(tankRightTrackTriggerCollider);
        isLeftTrackGrounded = CheckTrackGrounded(tankLeftTrackTriggerCollider);

        bool wPressed = Input.GetKey(KeyCode.W);
        bool sPressed = Input.GetKey(KeyCode.S);
        bool aPressed = Input.GetKey(KeyCode.A);
        bool dPressed = Input.GetKey(KeyCode.D);

        isMovingForward = wPressed && !sPressed;
        isMovingBackward = sPressed && !wPressed;
        isTurningRight = dPressed && !aPressed;
        isTurningLeft = aPressed && !dPressed;
        
        forwardSpeed = Vector3.Dot(tankRigidbody.linearVelocity, transform.forward);
        
        Vector3 rightTrackForce = CalculateTrackForce(true);
        Vector3 leftTrackForce = CalculateTrackForce(false);
        
        if (isRightTrackGrounded)
            tankRigidbody.AddForceAtPosition(rightTrackForce*movementDirection, forceRightTrackPositionWorld);
        
        if (isLeftTrackGrounded)
            tankRigidbody.AddForceAtPosition(leftTrackForce*movementDirection, forceLeftTrackPositionWorld);
        
        ApplySpeedLimit();
    }
    
    private void ApplySpeedLimit()
    {
        float currentForwardSpeed = Vector3.Dot(tankRigidbody.linearVelocity, transform.forward);
        
        if (currentForwardSpeed > maxSpeed)
        {
            Vector3 lateralVelocity = tankRigidbody.linearVelocity - transform.forward * currentForwardSpeed;
            tankRigidbody.linearVelocity = lateralVelocity + transform.forward * maxSpeed;
        }
        
        if (currentForwardSpeed < -maxReverseSpeed)
        {
            Vector3 lateralVelocity = tankRigidbody.linearVelocity - transform.forward * currentForwardSpeed;
            tankRigidbody.linearVelocity = lateralVelocity + transform.forward * -maxReverseSpeed;
        }
        
        if (Mathf.Abs(currentForwardSpeed) > maxSpeed * 0.9f)
        {
            tankRigidbody.linearVelocity *= speedLimiterFactor;
        }
    }

    private Vector3 CalculateTrackForce(bool isRightTrack)
    {
        float baseForce = isRightTrack ? tankRightTrackPullingForce : tankLeftTrackPullingForce;
        Vector3 force = Vector3.zero;
        float direction = 1f;
        
        if (isMovingForward) direction = 1f;
        else if (isMovingBackward) direction = -1f;
        
        float speedFactor = 1f;
        if (isMovingForward && forwardSpeed > maxSpeed * 0.8f)
            speedFactor = Mathf.Clamp01(1f - (forwardSpeed - maxSpeed * 0.8f) / (maxSpeed * 0.2f));
        else if (isMovingBackward && forwardSpeed < -maxReverseSpeed * 0.8f)
            speedFactor = Mathf.Clamp01(1f - (-forwardSpeed - maxReverseSpeed * 0.8f) / (maxReverseSpeed * 0.2f));

        baseForce *= speedFactor;


        if (isMovingForward && !isTurningLeft && !isTurningRight)
        {
            force = transform.forward * (baseForce * direction);
        }
        else if (isMovingBackward && !isTurningLeft && !isTurningRight)
        {
            force = transform.forward * (baseForce * direction);
        }
        else if (isTurningRight && !isMovingForward && !isMovingBackward)
        {
            force = isRightTrack ? 
                -transform.forward * (baseForce * turningWhileMovingForceMultiplier) : 
                transform.forward * (baseForce * turningWhileMovingForceMultiplier);
        }
        else if (isTurningLeft && !isMovingForward && !isMovingBackward)
        {
            force = isRightTrack ? 
                transform.forward * (baseForce * turningWhileMovingForceMultiplier) : 
                -transform.forward * (baseForce * turningWhileMovingForceMultiplier);
        }
        
        if (isMovingForward && isTurningRight)
        {
            force = isRightTrack ?
                 transform.forward * (baseForce * (1f - steeringFactor)):
                 transform.forward * (baseForce * (1f + steeringFactor));
        }
        else if (isMovingForward && isTurningLeft)
        {
            force = isRightTrack ?
                transform.forward * (baseForce * (1f + steeringFactor)):
                transform.forward * (baseForce * (1f - steeringFactor));
        }
        
        if (isMovingBackward && isTurningRight)
        {
            force = isRightTrack ? 
                -transform.forward * (baseForce * (1f - steeringFactor)):
                -transform.forward * (baseForce * (1f + steeringFactor));
        }
        else if (isMovingBackward && isTurningLeft)
        {
            force = isRightTrack ?
                -transform.forward * (baseForce * (1f + steeringFactor)):
                -transform.forward * (baseForce * (1f - steeringFactor));
        }
        
        return force;
    }

    private bool CheckTrackGrounded(BoxCollider trackCollider)
    {
        if (trackCollider == null) return false;
        
        Vector3 center = trackCollider.transform.TransformPoint(trackCollider.center);
        Vector3 halfExtents = trackCollider.size * 0.5f;
        Quaternion rotation = trackCollider.transform.rotation;
        
        return Physics.CheckBox(center, halfExtents, rotation, groundLayer);
    }
}