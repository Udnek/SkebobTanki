namespace ECS
{
    public class AbstractEntity : Entity
    {
        public ComponentMap components { get; } = new();
    }
}
