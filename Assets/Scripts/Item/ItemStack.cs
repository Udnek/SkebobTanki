using UnityEngine;

namespace Item
{
    public class ItemStack
    {
        public readonly ItemType type;

        public ItemStack(ItemType type)
        {
            this.type = type;
        }
    }
}
