using UnityEngine;

public class TankTurretController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 30f;
    public float rotationSmoothing = 10f; 


    private Quaternion targetRotation; 
    private float rotationInput; 

    void Start()
    {
        targetRotation = transform.localRotation;
    }

    void Update()
    {
        rotationInput = Input.GetAxis("Mouse X");
        if (rotationInput != 0)
        {
            float rotationAmount = rotationInput * rotationSpeed * Time.fixedDeltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0f, rotationAmount, 0f);
            targetRotation *= deltaRotation;
        }
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            rotationSmoothing * Time.fixedDeltaTime
        );
    }
}