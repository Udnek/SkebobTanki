using System.Collections.Generic;
using Item;
using Item.Components;
using JetBrains.Annotations;
using NUnit.Framework;
using UI;

namespace Inventory
{
    public class Slot
    {
        public readonly Inventory inventory;

        public readonly SlotType type;

        [CanBeNull] public Slot provider;
        
        public bool stillExists = true;
        [CanBeNull] public ItemStack item { get; private set; }
        public Slot(Inventory inventory, SlotType type)
        {
            this.inventory = inventory;
            this.type = type;
        }

        public void Swap(Slot withSlot)
        {
            if (withSlot == this) return;
            var thisItem = item;
            var withItem = withSlot.item;
            withSlot.item = thisItem;
            InternalSet(withItem, false);
            if (withSlot.stillExists) withSlot.InternalSet(thisItem, false);
            
            if (stillExists) inventory.OnItemSwapped(this, withSlot);
            if (withSlot.stillExists) withSlot.inventory.OnItemSwapped(withSlot, this);
        }

        public void Set(ItemStack newItem)
        {
            var oldItem = item;
            InternalSet(newItem, true);
            inventory.OnItemSet(this, oldItem);
        }

        public void SetNoLeftOver(ItemStack newItem)
        {
            var oldItem = item;
            InternalSet(newItem, false);
            inventory.OnItemSet(this, oldItem);
        }
        
        protected virtual void InternalSet([CanBeNull] ItemStack newItem, bool leftoverThisItem)
        {
            if (leftoverThisItem && item != null) inventory.TakeLeftover(this);
            item = newItem;
        }
    }
}