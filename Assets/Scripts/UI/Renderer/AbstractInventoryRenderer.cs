using System.Collections.Generic;
using Inventory;
using Item;
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
            var uiSlot = slots[slot];
            uiSlot.UpdateIcon();
            uiSlot.ResetIconPosition();
        }

        public void OnItemSwapped(Inventory.Slot thisSlot, Inventory.Slot withSlot)
        {
            // Debug.Log("this: " + (thisSlot.item?.type?.name ?? "null") + " | with: " + (withSlot.item?.type?.name ?? "null"));
            // Debug.Log($"THIS SLOT: {thisSlot.type.name} | WITH SLOT: {withSlot.type.name}");
            var manager = InventoryManager.instance;
            var withUiSlot = manager.GetSlot(withSlot);
            var thisUiSlot = slots[thisSlot];
            thisUiSlot.UpdateIcon();
            if (withUiSlot is null) {}
            else if (thisUiSlot.icon is not null)
            {
                var icon = thisUiSlot.icon!;
                manager.AddToDraggableLayer(icon);
                icon.transform.position = manager.currentlyMovingSlot == withUiSlot ? 
                    withUiSlot!.icon!.transform.position 
                    : withUiSlot.transform.position;
                SmoothMover.Run(
                    icon,
                    16,
                    thisUiSlot.transform.position,
                    () =>
                    {
                        thisUiSlot.UpdateIcon();
                        var list = slotsToAppear.GetValueOrDefault(thisSlot, null);
                        if (list is null) return;
                        
                        foreach (var slot in list)
                        {
                            if (slot !=null) SmoothSizeChange.Run(slot.gameObject, 0.2f, 1f);
                        }
                        slotsToAppear.Remove(thisSlot);
                    });
            }
        }

        public abstract void OnRowRemoved(StorageRow row);

        public abstract void OnRowAdded(StorageRow row);
        public void OnItemChanged(Inventory.Slot slot, ItemStack oldItem) { }
    }
}