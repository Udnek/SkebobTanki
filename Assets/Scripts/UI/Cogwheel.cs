using UnityEngine;

namespace UI
{
    public class Cogwheel : MonoBehaviour
    {
        public float speed = 1;
        public RectTransform mainImage;
        public RectTransform shadow;

        private void Update()
        {
            var angle = new Vector3(0, 0, speed * Time.deltaTime);
            mainImage.Rotate(angle);
            shadow.Rotate(angle);
        }
    }
}
