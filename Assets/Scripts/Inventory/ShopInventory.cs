using System.Collections.Generic;
using Item;
using Item.Components;
using NUnit.Framework;

namespace Inventory
{
    public class ShopInventory : Inventory
    {
        public Slot[] slots { get; }
        
        public ShopInventory()
        {
            var items = ItemManager.instance.all;
            slots = new Slot[items.Count];
            for (var index = 0; index < slots.Length; index++)
            {
                var slot = new Slot(this, SlotType.SHOP);
                slot.SetNoLeftOver(new ItemStack(items[index]));
                slots[index] = slot;
            }
        }

        public override void TakeLeftover(Slot slot) {}
    }
}