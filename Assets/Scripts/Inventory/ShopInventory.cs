using System.Collections.Generic;
using Item;

namespace Inventory
{
    public class ShopInventory : Inventory
    {
        public InventoryListener listener;
        public Slot[] slots { get; }

        public ShopInventory()
        {
            var items = ItemManager.instance.all;
            slots = new Slot[items.Count];
            for (var index = 0; index < slots.Length; index++)
            {
                var slot = new Slot(this);
                slot.SetNoLeftOver(new ItemStack(items[index]));
                slots[index] = slot;
            }
        }

        public void OnItemSet(Slot slot, ItemStack oldItem, List<ItemStack> leftOver)
        {
            listener?.OnItemSet(slot, oldItem, leftOver);
        }

        public void OnItemSwapped(Slot thisSlot, Slot withSlot, List<ItemStack> leftover) 
            => listener?.OnItemSwapped(thisSlot, withSlot, leftover);
        

        public void OnSlotRemoved(Slot slot, Row row) => listener?.OnSlotRemoved(slot, row);

        public void OnSlotAdded(Slot slot, Row row) => listener?.OnSlotAdded(slot, row);


        // public class ShopSlot: Slot
        // {
        //     public ShopSlot(Inventory inventory) : base(inventory) { }
        //
        //     public void ForceSet(ItemStack newItem) => item = newItem;
        //
        //     public override List<ItemStack> SetAndGetLeftover(ItemStack newItem, bool includeThisItemInLeftover = true)
        //     {
        //         var leftover = new List<ItemStack>();
        //         inventory.OnItemReplaced(this, item, leftover);
        //         return leftover;
        //     }
        // }
    }
}