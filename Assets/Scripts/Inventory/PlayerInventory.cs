using System;
using System.Collections.Generic;
using System.Linq;
using Item;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;

namespace Inventory
{
    public class PlayerInventory : MonoBehaviour, Inventory
    {
        public InventoryListener listener;
        public Row hullSlots { get; }
        public Row turretSlots { get; }
        public Slot[] backpack {get; } = new Slot[8];
        public Row[] rows => new[]{ hullSlots, turretSlots };
        

        public PlayerInventory()
        {
            hullSlots = new Row(this);
            turretSlots = new Row(this);
            for (var i = 0; i < backpack.Length; i++)
            {
                backpack[i] = new Slot(this);
            }
        }
        
        public void OnItemSet(Slot slot, ItemStack oldItem, List<ItemStack> leftover)
        {
            AddToBackpack(leftover);
            listener?.OnItemSet(slot, oldItem, leftover);
        }

        public void OnItemSwapped(Slot thisSlot, Slot withSlot, List<ItemStack> leftover)
        {
            AddToBackpack(leftover);
            listener?.OnItemSwapped(thisSlot, withSlot, leftover);
        }

        private void AddToBackpack(List<ItemStack> items)
        {
            foreach (var backSlot in backpack)
            {
                if (items.Count == 0) break;
                if (backSlot.item != null) continue;
                backSlot.Set(items.Pop());
            }
        }

        public void OnSlotRemoved(Slot slot, Row row) => listener?.OnSlotRemoved(slot, row);
        public void OnSlotAdded(Slot slot, Row row) => listener?.OnSlotAdded(slot, row);
    }
}
