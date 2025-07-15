using UnityEngine;

namespace Test
{
    public class TurretRotation : MonoBehaviour {
        public float sensitivity = 1f;
        private void Update() {
            float rotationX = Input.GetAxis("Mouse X") * sensitivity;
            transform.Rotate(Vector3.up, rotationX, Space.World);
        }
    }
}

