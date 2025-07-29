using Item.Components;
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
            consumer(SlotType.BOTTOM, bottomPosition);
            consumer(SlotType.TOP, topPosition);
            consumer(SlotType.LEFT, leftPosition);
            consumer(SlotType.RIGHT, rightPosition);
        }
    }
}