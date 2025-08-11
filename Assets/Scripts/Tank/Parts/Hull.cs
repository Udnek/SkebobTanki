using Inventory.SlotTypes;
using UnityEngine;

namespace Tank.Parts
{
    public class Hull : BasePart
    {
        public Transform bottomPosition;
        public Transform topPosition;
        public Transform leftPosition;
        public Transform rightPosition;
        

        protected override void AddSlots(SlotConsumer consumer)
        {
            consumer(SlotManager.instance.BOTTOM, bottomPosition);
            consumer(SlotManager.instance.TOP, topPosition);
            consumer(SlotManager.instance.LEFT, leftPosition);
            consumer(SlotManager.instance.RIGHT, rightPosition);
        }
    }
}