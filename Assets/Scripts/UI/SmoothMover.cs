using System;
using UnityEngine;

namespace UI
{
    public class SmoothMover : MonoBehaviour
    {
        private float step;
        public float speed;
        public Vector3 startPosition;
        public Vector3 target;
        public Action onArrival;
        
        public static void Run(GameObject obj, float speed, Vector3 target, Action onArrival)
        {
            var component = obj.AddComponent<SmoothMover>();
            component.speed = speed;
            component.target = target;
            component.onArrival = onArrival;
        }

        public void Start()
        {
            startPosition = transform.position;
        }

        public void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            step += Time.deltaTime * speed;
            if (Vector3.Distance(transform.position, target) > 0.001f) return;
            Destroy(this);
            onArrival?.Invoke();
        }
    }
}