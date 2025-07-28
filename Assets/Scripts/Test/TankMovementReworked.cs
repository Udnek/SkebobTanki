using System;
using UnityEngine;

public class TankMovementReworked : MonoBehaviour
{
    [SerializeField] private float tankMass;
    [SerializeField] private float tankPullingForce;
    [SerializeField] private LayerMask groundLayer; // Слой для определения поверхности

    [SerializeField] private Rigidbody tankRigidbody;

    [SerializeField] private BoxCollider tankRightTrackTriggerCollider;
    [SerializeField] private BoxCollider tankLeftTrackTriggerCollider;

    private float tankRightTrackPullingForce;
    private float tankLeftTrackPullingForce;
    private float tankRightTrackMass;
    private float tankLeftTrackMass;
    
    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    private bool isTurningRight = false;
    private bool isTurningLeft = false;
    
    public Vector3 forceLeftTrackPositionLocal = new Vector3(1.5f, 0, 0);
    public Vector3 forceRightTrackPositionLocal = new Vector3(-1.5f, 0, 0);

    private Vector3 forceRightTrackPositionWorld;
    private Vector3 forceLeftTrackPositionWorld;

    [SerializeField] private float reverseDirection = 1;

    // Флаги касания поверхности
    private bool isRightTrackGrounded;
    private bool isLeftTrackGrounded;

    private void Start()
    {
        tankRightTrackPullingForce = 0.5f * tankPullingForce;
        tankLeftTrackPullingForce = 0.5f * tankPullingForce;
        
        tankRightTrackMass = 0.5f * tankMass;
        tankLeftTrackMass = 0.5f * tankMass;

        // Автоматическая настройка слоя по умолчанию
        if (groundLayer.value == 0)
            groundLayer = LayerMask.GetMask("Default");
    }
    
    private void FixedUpdate()
    {
        // Проверяем касание поверхности для гусениц
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
        
        RightTrackMovement(wPressed, sPressed, aPressed, dPressed);
        LeftTrackMovement(wPressed, sPressed, aPressed, dPressed);
    }

    // Проверка касания поверхности для гусеницы
    private bool CheckTrackGrounded(BoxCollider trackCollider)
    {
        if (trackCollider == null) return false;
        
        Vector3 center = trackCollider.transform.TransformPoint(trackCollider.center);
        Vector3 halfExtents = trackCollider.size * 0.5f;
        Quaternion rotation = trackCollider.transform.rotation;
        
        return Physics.CheckBox(
            center, 
            halfExtents, 
            rotation, 
            groundLayer
        );
    }

    public void RightTrackMovement(bool wPressed, bool sPressed, bool aPressed, bool dPressed)
    {
        // Заменяем проверку isTrigger на проверку касания земли
        if (isRightTrackGrounded)
        {
            Vector3 rightTrackForce = transform.forward * (reverseDirection * tankRightTrackPullingForce);
            if (isMovingForward)
            {
                rightTrackForce *= 1;
                if (dPressed)
                {
                    rightTrackForce *= 0.5f;
                }
                else if (aPressed)
                {
                    rightTrackForce *= 1;
                }
            }
            else if (isMovingBackward)
            {
                rightTrackForce *= -1;
                if (dPressed)
                {
                    rightTrackForce *= 0.5f;
                }
                else if (aPressed)
                {
                    rightTrackForce *= 1;
                }
                
            }
            else if (isTurningRight && !wPressed && !sPressed)
            {
                rightTrackForce *= -1;
            }
            else if (isTurningLeft && !wPressed && !sPressed)
            {
                rightTrackForce *= 1;
            }
            else
            {
                rightTrackForce *= 0;
            }
            forceRightTrackPositionWorld = transform.TransformPoint(forceRightTrackPositionLocal);
            tankRigidbody.AddForceAtPosition(rightTrackForce, forceRightTrackPositionWorld);
        }
    }

    public void LeftTrackMovement(bool wPressed, bool sPressed, bool aPressed, bool dPressed)
    {
        // Заменяем проверку isTrigger на проверку касания земли
        if (isLeftTrackGrounded)
        {
            Vector3 leftTrackForce = transform.forward * (reverseDirection * tankLeftTrackPullingForce);
            if (isMovingForward)
            {
                leftTrackForce *= 1;
                if (dPressed)
                {
                    leftTrackForce *= 1;
                }
                else if (aPressed)
                {
                    leftTrackForce *= 0.5f;
                }
                
            }
            else if (isMovingBackward)
            {
                leftTrackForce *= -1;
                if (dPressed)
                {
                    leftTrackForce *= 1;
                }
                else if (aPressed)
                {
                    leftTrackForce *= 0.5f;
                }
                
            } 
            else if (isTurningRight && !wPressed && !sPressed)
            {
                leftTrackForce *= 1;
            }
            else if (isTurningLeft && !wPressed && !sPressed)
            {
                leftTrackForce *= -1;
            }
            else
            {
                leftTrackForce *= 0;
            }
            forceLeftTrackPositionWorld = transform.TransformPoint(forceLeftTrackPositionLocal);
            tankRigidbody.AddForceAtPosition(leftTrackForce, forceLeftTrackPositionWorld);
        }
    }
}