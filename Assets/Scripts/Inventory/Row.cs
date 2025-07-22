using System;
using System.Collections.Generic;
using Item;
using JetBrains.Annotations;

namespace Inventory
{
    public class Row
    {
        public MainSlot main { get; private set; }
        public Slot[] extra { get; private set; } = Array.Empty<Slot>();

        internal Row(PlayerInventory inventory) => main = new MainSlot(inventory, this);
        

        public class MainSlot : Slot
        {
            private readonly Row row;
            internal MainSlot(PlayerInventory inventory, Row row) : base(inventory) => this.row = row;

            protected override List<ItemStack> SetAndGetLeftover(ItemStack newItem, bool includeThisItemInLeftover)
            {
                var leftover = base.SetAndGetLeftover(newItem, includeThisItemInLeftover);
                foreach (var slot in row.extra)
                {
                    if (slot.item != null) leftover.Add(slot.item);
                    slot.stillExists = false;
                    inventory.OnSlotRemoved(slot, row);
                }
                row.extra = new Slot[item?.type?.extraSlots ?? 0];
                for (var i = 0; i < row.extra.Length; i++)
                {
                    var slot = new Slot(inventory);
                    row.extra[i] = slot;
                    inventory.OnSlotAdded(slot, row);
                }
                return leftover;
            }
        }
    }
}