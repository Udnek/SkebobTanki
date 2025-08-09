using System.Collections.Generic;
using Item;
using Item.Components;
using JetBrains.Annotations;

namespace Inventory
{
    public partial class PlayerInventory : Inventory
    {
        public StorageSlot hullSlot { get; }
        public Slot[] backpack {get; } = new Slot[8];
        public List<StorageRow> storageRows { get; } = new();

        public PlayerInventory()
        {
            hullSlot = new StorageSlot(this, SlotType.HULL);
            for (var i = 0; i < backpack.Length; i++)
            {
                backpack[i] = new Slot(this, SlotType.BACKPACK);
            }
        }
        
        public override void TakeLeftover(Slot slot) => AddToBackpack(slot);
        private void AddToBackpack(Slot slot)
        {
            if (slot.item == null) return;
            foreach (var backSlot in backpack)
            {
                if (backSlot.item != null) continue;
                backSlot.Swap(slot);
                return;
            }
            slot.SetNoLeftOver(null);
        }

        [CanBeNull]
        public Slot AddToBackpack(ItemStack stack)
        {
            foreach (var slot in backpack)
            {
                if (slot.item != null) continue;
                slot.SetNoLeftOver(stack);
                return slot;
            }
            return null;
        }
    }
}



















