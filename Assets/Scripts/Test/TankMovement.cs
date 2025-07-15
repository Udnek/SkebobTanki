using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 45f;
    public float acceleration = 2f;
    public float deceleration = 4f;
    public float rotationAcceleration = 90f;
    
    private float horizontalInput;
    private float verticalInput;
    private float currentSpeed = 0f;
    private float currentRotation = 0f;
    
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        if (verticalInput != 0)
        {
            currentSpeed = Mathf.Lerp(
                currentSpeed, 
                verticalInput * moveSpeed, 
                acceleration * Time.deltaTime
            );
        }
        else
        {
            currentSpeed = Mathf.Lerp(
                currentSpeed, 
                0f, 
                deceleration * Time.deltaTime
            );
        }
        
        if (horizontalInput != 0)
        {
            currentRotation = Mathf.Lerp(
                currentRotation, 
                horizontalInput * rotationSpeed, 
                rotationAcceleration * Time.deltaTime
            );
        }
        else
        {
            currentRotation = Mathf.Lerp(
                currentRotation, 
                0f, 
                rotationAcceleration * Time.deltaTime
            );
        }

        transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime * -1));
        transform.Rotate(Vector3.up * (currentRotation * Time.deltaTime));

        if (!(Mathf.Abs(currentSpeed) > 0.1f) && !(Mathf.Abs(currentRotation) > 0.1f)) return;
        
        float vibrationIntensity = Mathf.Clamp01((Mathf.Abs(currentSpeed) + Mathf.Abs(currentRotation)) / 10f);
        transform.position += Random.insideUnitSphere * (0.01f * vibrationIntensity);
    }
}