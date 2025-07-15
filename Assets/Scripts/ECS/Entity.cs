using UnityEngine;

namespace ECS
{
    public abstract class Entity : MonoBehaviour
    {
        public ComponentMap components {get; private set;}

        public void Awake() => components = ECSystem.Instance.GetOrCreate(gameObject);
    }
}
