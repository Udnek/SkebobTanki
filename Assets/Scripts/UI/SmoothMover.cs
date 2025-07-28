using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UI
{
    public class SmoothMover : MonoBehaviour
    {

        private Action update;
        public static void Run(GameObject obj, float speed, Vector3 target, [CanBeNull] Action onArrival = null)
        {
            var component = obj.AddComponent<SmoothMover>();
            float step = 0;
            component.update = () =>
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, step);
                step += Time.deltaTime * speed;
                if (Vector3.Distance(obj.transform.position, target) > 0.001f) return;
                Destroy(obj.GetComponent<SmoothMover>());
                onArrival?.Invoke();
            };
        }

        public static void RunLocal(GameObject obj, float speed, Vector3 target, [CanBeNull] Action onArrival = null)
        {
            var component = obj.AddComponent<SmoothMover>();
            float step = 0;
            component.update = () =>
            {
                obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, target, step);
                step += Time.deltaTime * speed;
                if (Vector3.Distance(obj.transform.localPosition, target) > 0.001f) return;
                Destroy(obj.GetComponent<SmoothMover>());
                onArrival?.Invoke();
            };
        }
        
        public void Update() => update.Invoke();
    }
}