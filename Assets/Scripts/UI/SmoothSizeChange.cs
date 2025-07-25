using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    // TODO USE ANIMATOR INSTEAD
    public class SmoothSizeChange : MonoBehaviour
    {
        private float time;
        private float multiplier;
        public float targetSize;
        public float duration;
        public GameObject obj;
        
        public static void Run(GameObject obj, float duration, float targetSize)
        {
            var component = obj.AddComponent<SmoothSizeChange>();
            component.duration = duration;
            component.obj = obj;
            component.targetSize = targetSize;
        }

        private void Start()
        {
            multiplier = obj.transform.localScale.x < targetSize ? 1 : -1;
            obj.transform.localScale = new Vector3();
        }

        public void Update()
        {
            var size = targetSize / duration * Time.deltaTime * multiplier;
            time += Time.deltaTime;
            obj.transform.localScale += new Vector3(size, size, size);
            if (time < duration) return;
            obj.transform.localScale = new Vector3(targetSize, targetSize, targetSize);
            Destroy(this);
        }
    }
}