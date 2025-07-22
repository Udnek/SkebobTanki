using UnityEngine;

namespace Tank
{
    public class Turret : Part
    {
        [field: SerializeField]
        public Transform barrelPosition { get; private set; }
    }
}