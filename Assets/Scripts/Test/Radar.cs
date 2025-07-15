using UnityEngine;

namespace Test
{
    public class Radar : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 90f);
        }
    }
}
