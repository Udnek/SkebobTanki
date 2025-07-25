using System;
using System.Collections.Generic;
using System.Linq;
using Item;
using Item.Components;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;

namespace Inventory
{
    public class PlayerInventory : Inventory
    {
        public Row hull { get; }
        public Row turret { get; }
        public Slot[] backpack {get; } = new Slot[8];
        public Row[] rows => new[]{ hull, turret };

        public PlayerInventory()
        {
            hull = new Row(this, SlotType.HULL);
            turret = new Row(this, SlotType.TOP);
            turret.main.provider = hull.main;
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
    }
}
