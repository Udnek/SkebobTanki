namespace ECS
{
    public class AbstractEntity<THolder> : Entity<THolder>
    {
        private ComponentMap<THolder> componentsField = new();
        public ComponentMap<THolder> components => componentsField ??= new ComponentMap<THolder>();
    }
}
