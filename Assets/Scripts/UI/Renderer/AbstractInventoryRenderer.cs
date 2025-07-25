using System.Collections.Generic;
using Inventory;
using Item;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

namespace UI.Renderer
{
    public abstract class AbstractInventoryRenderer<T> : InventoryListener, InventoryRenderer where T : Inventory.Inventory
    {
        protected readonly Dictionary<Inventory.Slot, Slot> slots = new();
        protected abstract T inventory { get; set; }
        
        protected Dictionary<Inventory.Slot, List<Slot>> slotsToAppear = new();
        protected Dictionary<Inventory.Slot, List<Slot>> slotsToDisappear = new();
        
        public Slot GetSlot(Inventory.Slot slot) => slots.GetValueOrDefault(slot, null);

        public virtual void Close()
        {
            inventory.Unsubscribe(this);
            foreach (var slot in slots.Values)
            {
                slot.Clear();
                Object.Destroy(slot.gameObject);
            }
        }

        public virtual void Open()
        {
            inventory.Subscribe(this);
        }

        public void OnItemSet(Inventory.Slot slot, ItemStack oldItem)
        {
            slots[slot].UpdateIcon();
        }

        public void OnItemSwapped(Inventory.Slot thisSlot, Inventory.Slot withSlot)
        {
            var manager = InventoryManager.instance;
            var withVisualSlot = manager.GetSlot(withSlot);
            var thisVisualSlot = slots[thisSlot];
            thisVisualSlot.UpdateIcon();
            if (withVisualSlot is null) { }
            else if (thisVisualSlot.icon is not null)
            {
                var icon = thisVisualSlot.icon!;
                manager.AddToDraggableLayer(icon);
                icon.transform.position = manager.currentlyMovingSlot == withVisualSlot ? 
                    withVisualSlot!.icon!.transform.position 
                    : withVisualSlot.transform.position;
                SmoothMover.Run(
                    icon,
                    16,
                    thisVisualSlot.transform.position,
                    () =>
                    {
                        thisVisualSlot.UpdateIcon();
                        var list = slotsToAppear.GetValueOrDefault(thisSlot, null);
                        if (list is null) return;
                        
                        foreach (var slot in list)
                        {
                            if (slot !=null) SmoothSizeChange.Run(slot.gameObject, 0.5f, 1f);
                        }
                        slotsToAppear.Remove(thisSlot);
                    });
            }
        }

        public void OnSlotRemoved(Inventory.Slot slot, Row row)
        {
            var inventorySlot = slots[slot];
            inventorySlot.Clear();
            Object.Destroy(inventorySlot.gameObject);
            slots.Remove(slot);
        }

        public abstract void OnSlotAdded(Inventory.Slot slot, Row row);
        public void OnItemChanged(Inventory.Slot slot, ItemStack oldItem) { }
    }
}