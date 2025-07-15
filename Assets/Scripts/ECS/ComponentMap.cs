#nullable enable
using System.Collections.Generic;

namespace ECS
{
    public class ComponentMap
    {
        private List<CustomComponent> components = new();
        
        public T? Get<T>() where T : CustomComponent
        {
            return components.Find(component => component is T) as T;
        }
        
        public bool Has<T>() where T: CustomComponent => Get<T>() != null;
        public void Add(CustomComponent component) => components.Add(component);
    }
}