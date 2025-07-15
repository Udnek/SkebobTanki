using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ECS
{
    public class ECSystem
    {
        private static ECSystem instance;
        public static ECSystem Instance => instance ??= new ECSystem();
        
        private readonly Dictionary<GameObject, ComponentMap> data = new();
        
        private ECSystem() {}
        
        public ComponentMap GetOrCreate(GameObject gameObject)
        {
            var map = Get(gameObject);
            if (map == null) return data[gameObject] = new ComponentMap();
            return map;
        }
        
        [CanBeNull]
        public ComponentMap Get(GameObject gameObject) => data.GetValueOrDefault(gameObject, null);
    }
    
}
