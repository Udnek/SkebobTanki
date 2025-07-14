using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 45f;
    public float acceleration = 2f;
    public float deceleration = 4f;
    public float rotationAcceleration = 90f;
    
    private float _horizontalInput;
    private float _verticalInput;
    private float _currentSpeed = 0f;
    private float _currentRotation = 0f;
    
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        
        if (_verticalInput != 0)
        {
            _currentSpeed = Mathf.Lerp(
                _currentSpeed, 
                _verticalInput * moveSpeed, 
                acceleration * Time.deltaTime
            );
        }
        else
        {
            _currentSpeed = Mathf.Lerp(
                _currentSpeed, 
                0f, 
                deceleration * Time.deltaTime
            );
        }
        
        if (_horizontalInput != 0)
        {
            _currentRotation = Mathf.Lerp(
                _currentRotation, 
                _horizontalInput * rotationSpeed, 
                rotationAcceleration * Time.deltaTime
            );
        }
        else
        {
            _currentRotation = Mathf.Lerp(
                _currentRotation, 
                0f, 
                rotationAcceleration * Time.deltaTime
            );
        }

        transform.Translate(Vector3.forward * _currentSpeed * Time.deltaTime * -1);
        transform.Rotate(Vector3.up * _currentRotation * Time.deltaTime);
        
        if (Mathf.Abs(_currentSpeed) > 0.1f || Mathf.Abs(_currentRotation) > 0.1f)
        {
            float vibrationIntensity = Mathf.Clamp01((Mathf.Abs(_currentSpeed) + Mathf.Abs(_currentRotation)) / 10f);
            transform.position += Random.insideUnitSphere * 0.01f * vibrationIntensity;
        }
    }
}