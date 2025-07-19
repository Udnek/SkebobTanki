using UnityEngine;

namespace Tank
{
    public record HitEvent
    {
        public readonly RaycastHit hit;
        public readonly float force;
        public readonly Vector3 direction;

        public HitEvent(RaycastHit hit, float force, Vector3 direction)
        {
            this.hit = hit;
            this.force = force;
            this.direction = direction;
        }
    }
}