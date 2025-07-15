using System.Collections.Generic;
using ECS;
using JetBrains.Annotations;

namespace Item
{
    public class Inventory : CustomComponent
    {
        private ItemStack[] items = new ItemStack[3];
        
        public void Clear() => items = new ItemStack[3];
        public void SetItem(int index, ItemStack itemStack) => items[index] = itemStack;
        
        [CanBeNull] public ItemStack GetItem(int index) => items[index];
        public ItemStack[] GetItems() => items;
    }
}
