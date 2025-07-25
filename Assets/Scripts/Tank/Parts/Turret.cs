using Item.Components;
using UnityEngine;

namespace Tank.Parts
{
    public class Turret : BasePart
    {
        [field: SerializeField]
        public Transform barrelPosition { get; private set; }

        protected override void AddSlots(SlotConsumer consumer)
        {
            consumer(SlotType.FRONT, barrelPosition);
        }
    }
}