#nullable enable
using System.Collections.Generic;

namespace ECS
{
    public class ComponentMap<THolder>
    {
        public delegate T ComponentCreator<out T>() where T : CustomComponent<THolder>;
        
        private List<CustomComponent<THolder>>? components;
        
        public T? Get<T>() where T : CustomComponent<THolder>
        {
            return (T?)components?.Find(component => component is T);
        }
        public bool Has<T>() where T: CustomComponent<THolder> => Get<T>() != null;

        public T GetOrAdd<T>(ComponentCreator<T> creator) where T : CustomComponent<THolder>
        {
            var component = Get<T>();
            if (component != null) return component;
            component = creator();
            Add(component);
            return component;
        }
        
        public void Add(CustomComponent<THolder> component)
        {
            components ??= new List<CustomComponent<THolder>>();
            components.Add(component);
        }
    }
}