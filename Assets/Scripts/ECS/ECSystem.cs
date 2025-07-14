using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    public class ECSystem
    {
        private static ECSystem instance;
        public static ECSystem Instance => instance ??= new ECSystem();
        
        private Dictionary<GameObject, ComponentMap> data = new();
        private ECSystem() {}
        
        public ComponentMap Get(GameObject gameObject)
        {
            var map = data.GetValueOrDefault(gameObject, null);
            if (map == null) return data[gameObject] = new ComponentMap();
            return map;
        }
    }
    
}
