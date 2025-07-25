using System;
using System.Collections.Generic;
using Item;
using Item.Components;
using JetBrains.Annotations;
using UnityEngine;

namespace Inventory
{
    public class Row
    {
        public MainSlot main { get; }
        public Slot[] extra { get; private set; } = Array.Empty<Slot>();

        internal Row(PlayerInventory inventory, SlotType mainSlotType) 
            => main = new MainSlot(inventory, mainSlotType, this);


        // public class HullSlot : MainSlot
        // {
        //     internal HullSlot(PlayerInventory inventory, SlotType type, Row row) : base(inventory, type, row) {}
        //
        //     protected override void InternalSet(ItemStack newItem, bool leftoverThisItem)
        //     {
        //         base.InternalSet(newItem, leftoverThisItem);
        //         if (item == null)
        //         {
        //             (inventory as PlayerInventory).hull;
        //         }
        //     }
        // }
        
        public class MainSlot : Slot
        {
            private readonly Row row;
            internal MainSlot(PlayerInventory inventory, SlotType type, Row row) : base(inventory, type) => this.row = row;

            protected override void InternalSet(ItemStack newItem, bool leftoverThisItem)
            {
                base.InternalSet(newItem, leftoverThisItem);

                for (var i = 0; i < row.extra.Length; i++)
                {
                    var slot = row.extra[i];
                    if (slot.item != null) inventory.TakeLeftover(slot);
                    slot.stillExists = false;
                    inventory.OnSlotRemoved(slot, row);
                }

                var slotTypes = item?.type?.components?.Get<ExtraSlots>()?.slots;
                if (slotTypes == null)
                {
                    row.extra = Array.Empty<Slot>();
                    return;
                }
                row.extra = new Slot[slotTypes.Length];
                for (var i = 0; i < slotTypes.Length; i++)
                {
                    var slot = new Slot(inventory, slotTypes[i]);
                    slot.provider = row.main;
                    row.extra[i] = slot;
                    inventory.OnSlotAdded(slot, row);
                }


                /*if (newExtraSize == row.extra.Length) return;

                for (var i = row.extra.Length - 1; i >= newExtraSize; i--)
                {
                    var slot = row.extra[i];
                    if (slot.item != null) inventory.TakeLeftover(slot);
                    slot.stillExists = false;
                    inventory.OnSlotRemoved(slot, row);
                }

                var extra = row.extra;
                var extraOldSize = extra.Length;
                Array.Resize(ref extra, newExtraSize);
                row.extra = extra;

                if (newExtraSize <= extraOldSize) return;
                for (var i = extraOldSize; i < row.extra.Length; i++)
                {
                    var slot = new Slot(inventory,);
                    row.extra[i] = slot;
                    inventory.OnSlotAdded(slot, row);
                }*/
            }
        }
    }
}