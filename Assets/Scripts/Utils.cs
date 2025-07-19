using UnityEngine;

namespace DefaultNamespace
{
    public static class Utils
    {
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            var component = obj.GetComponent<T>();
            if(component == null) component = obj.AddComponent<T>();
            return component;
        }

        public static T GetOrAddComponent<T>(this Component obj) where T : Component
        {
            return GetOrAddComponent<T>(obj.gameObject);
        }
    }
}