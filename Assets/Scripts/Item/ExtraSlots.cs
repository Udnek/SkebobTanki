using System.Collections.Generic;
using ECS;

namespace Item.Components
{
    public class ExtraSlots: CustomComponent<ItemType>
    {
        public SlotType[] slots { get; }
        
        public ExtraSlots(params SlotType[] slots)
        {
            this.slots = slots;
        }
    }
}