using UnityEngine;

namespace Tank
{
    public class Hull : Part
    {
        [field: SerializeField]
        public Transform tracksPosition { get; private set; }
        [field: SerializeField]
        public Transform turretPosition { get; private set; }
    }
}