namespace ECS
{
    public interface Entity<T>
    {
        ComponentMap<T> components { get; }
    }
}
