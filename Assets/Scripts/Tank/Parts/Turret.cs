using Inventory.SlotTypes;
using UnityEngine;

namespace Tank.Parts
{
    public class Turret : BasePart
    {
        [field: SerializeField]
        public Transform barrelPosition { get; private set; }

        protected override void AddSlots(SlotConsumer consumer)
        {
            consumer(SlotManager.instance.FRONT, barrelPosition);
        }
    }
}