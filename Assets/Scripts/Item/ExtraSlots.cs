using System;
using ECS;
using Inventory.SlotTypes;

namespace Item
{
    public class ExtraSlots: CustomComponent<ItemType>
    {
        public SlotType[] slots { get; }
        
        public ExtraSlots(params SlotType[] slots)
        {
            if (slots.Length == 0) throw new ArgumentException("slots cannot be empty!");
            this.slots = slots;
        }
    }
}