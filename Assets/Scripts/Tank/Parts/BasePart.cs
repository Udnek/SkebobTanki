using UnityEngine;

namespace Tank.Parts
{
    public class BasePart : Part
    {
        public int rotateAroundX;
        protected override void AddSlots(SlotConsumer consumer) { }

        public override void ApplyRotation()
        {
            transform.localRotation = Quaternion.identity;
            transform.Rotate(new Vector3(rotateAroundX, 0, 0));
        }
    }
}