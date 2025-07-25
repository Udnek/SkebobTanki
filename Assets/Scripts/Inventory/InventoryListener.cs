using System;
using System.Collections.Generic;
using Item;
using JetBrains.Annotations;

namespace Inventory
{
    public interface InventoryListener
    {
        void OnItemSet(Slot slot, [CanBeNull] ItemStack oldItem);
        void OnItemSwapped(Slot thisSlot, Slot withSlot);
        void OnSlotRemoved(Slot slot, Row row);
        void OnSlotAdded(Slot slot, Row row);
        void OnItemChanged(Slot slot, [CanBeNull] ItemStack oldItem);
    }
}