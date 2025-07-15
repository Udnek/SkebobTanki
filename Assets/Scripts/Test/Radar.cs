using UnityEngine;

public class Radar : MonoBehaviour
{
    
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 90f);
    }
}
