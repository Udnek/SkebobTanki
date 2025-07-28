using Item;
using Item.Components;
using JetBrains.Annotations;
using UnityEngine;

namespace Inventory
{
    public class Slot
    {
        public Inventory inventory { get; }
        public SlotType type { get; private set; }
        [CanBeNull] public Slot parent { get; set; }
        public bool stillExists { get; private set; } = true;
        [CanBeNull] public ItemStack item { get; private set; }
        public Slot(Inventory inventory, SlotType type, Slot parent = null)
        {
            this.inventory = inventory;
            this.type = type;
            this.parent = parent;
        }

        public void Destroy()
        {
            if (item != null) inventory.TakeLeftover(this);
            stillExists = false;
        }
        
        public void Swap(Slot withSlot)
        {
            if (withSlot == this) return;
            var thisItem = item;
            var withItem = withSlot.item;
            
            withSlot.item = thisItem;
            item = withItem;
            inventory.OnItemSwapped(this, withSlot);
            withSlot.inventory.OnItemSwapped(withSlot, this);

            item = thisItem;
            InternalSet(withItem, false);
            if (!withSlot.stillExists) return;
            withSlot.item = withItem;
            withSlot.InternalSet(thisItem, false);
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