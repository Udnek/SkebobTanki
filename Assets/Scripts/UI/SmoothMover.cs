using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UI.Managers
{
    public class SmoothMover : MonoBehaviour
    {
        private Action update;

        private static void Run(GameObject obj, Func<Boolean> shouldStop, Action updatePosition, [CanBeNull] Action onArrival = null)
        {
            var component = obj.AddComponent<SmoothMover>();
            component.update = () =>
            {
                updatePosition.Invoke();
                if (!shouldStop()) return;
                Destroy(obj.GetComponent<SmoothMover>());
                onArrival?.Invoke();
            };
        }
        
        public static void RunRect(GameObject obj, float speed, Vector2 target, [CanBeNull] Action onArrival = null)
        {
            var rectTransform = obj.GetComponent<RectTransform>();
            float step = 0;
            Run(obj,
                () => Vector2.Distance(rectTransform.anchoredPosition, target) < 0.001f,
                () =>
                {
                    rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, target, step);
                    step += Time.deltaTime * speed;
                },
                onArrival);
        }
        
        public static void Run(GameObject obj, float speed, Vector3 target, [CanBeNull] Action onArrival = null)
        {
            float step = 0;
            Run(obj,
                () => Vector3.Distance(obj.transform.position, target) < 0.001f,
                () =>
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, step);
                    step += Time.deltaTime * speed;
                },
                onArrival);
        }

        public static void RunLocal(GameObject obj, float speed, Vector3 target, [CanBeNull] Action onArrival = null)
        {
            float step = 0;
            Run(obj,
                () => Vector3.Distance(obj.transform.localPosition, target) < 0.001f,
                () =>
                {
                    obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, target, step);
                    step += Time.deltaTime * speed;
                },
                onArrival);
        }
        
        public void Update() => update.Invoke();
    }
}