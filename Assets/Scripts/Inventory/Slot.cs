using System.Collections.Generic;
using Item;
using JetBrains.Annotations;
using NUnit.Framework;
using UI;

namespace Inventory
{
    public class Slot
    {
        protected readonly Inventory inventory;

        public bool stillExists = true;
        [CanBeNull] public ItemStack item { get; protected set; }
        public Slot(Inventory inventory) => this.inventory = inventory;

        public void Swap(Slot withSlot)
        {
            var leftover = SwapAndGetLeftover(withSlot);
            if (stillExists) inventory.OnItemSwapped(this, withSlot, leftover);
            if (withSlot.stillExists) withSlot.inventory.OnItemSwapped(withSlot, this, leftover);
        }

        public void Set(ItemStack newItem)
        {
            inventory.OnItemSet(this, item, SetAndGetLeftover(newItem, true));
        }

        public void SetNoLeftOver(ItemStack newItem)
        {
            SetAndGetLeftover(newItem, false);
            inventory.OnItemSet(this, item, new List<ItemStack>());
        }
        
        protected virtual List<ItemStack> SwapAndGetLeftover(Slot withSlot)
        {
            var thisItem = item;
            var thatItem = withSlot.item;
            withSlot.item = null;
            var leftover = SetAndGetLeftover(thatItem, false);
            if (withSlot.stillExists) leftover.AddRange(withSlot.SetAndGetLeftover(thisItem, false));
            else leftover.Add(thisItem);
            return leftover;
        }

        protected virtual List<ItemStack> SetAndGetLeftover([CanBeNull] ItemStack newItem, bool includeThisItemInLeftover)
        {
            var oldItem = item;
            item = newItem;
            var leftOver = new List<ItemStack>();
            if (oldItem != null && includeThisItemInLeftover) leftOver.Add(oldItem);
            return leftOver;
        }
    }
}