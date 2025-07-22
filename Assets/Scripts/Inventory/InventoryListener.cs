using System.Collections.Generic;
using Item;
using JetBrains.Annotations;

namespace Inventory
{
    public interface InventoryListener
    {
        void OnItemSet(Slot slot, [CanBeNull] ItemStack oldItem, List<ItemStack> leftover);
        void OnItemSwapped(Slot thisSlot, Slot withSlot, List<ItemStack> leftover);
        void OnSlotRemoved(Slot slot, Row row);
        void OnSlotAdded(Slot slot, Row row);
    }
}