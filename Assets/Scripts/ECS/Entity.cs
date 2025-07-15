using UnityEngine;

namespace ECS
{
    public abstract class Entity : MonoBehaviour
    {
        protected ComponentMap components;

        public void Awake() => components = ECSystem.Instance.Get(gameObject);
    }
}
