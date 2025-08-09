using System.Collections.Generic;
using Inventory;
using Item;
using UI.Managers;
using UI.Slots;
using UnityEngine;

namespace UI.InventoryAgents
{
    public abstract class AbstractInventoryAgent<T> : InventoryListener, InventoryAgent, SlotListener where T : Inventory.Inventory
    {
        protected readonly Dictionary<Slot, InventorySlot> slots = new();
        protected abstract T inventory { get; set; }
        
        protected Dictionary<Slot, List<InventorySlot>> slotsToAppear = new();
        protected Dictionary<Slot, List<InventorySlot>> slotsToDisappear = new();
        
        public InventorySlot GetSlot(Slot slot) => slots.GetValueOrDefault(slot, null);

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

        public void OnItemSet(Slot slot, ItemStack oldItem)
        {
            var uiSlot = slots[slot];
            uiSlot.UpdateIcon();
            uiSlot.ResetIconPosition();
        }

        public void OnItemSwapped(Slot thisSlot, Slot withSlot)
        {
            var manager = TemporalManager.instance;
            var withUiSlot = InventoryManager.instance.GetSlot(withSlot);
            var thisUiSlot = slots[thisSlot];
            if (withUiSlot is null) thisUiSlot.UpdateIcon();
            else manager.SwapThisSlotAnimation(thisUiSlot, withUiSlot, 
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

        public abstract void OnRowRemoved(StorageRow row);
        public abstract void OnRowAdded(StorageRow row);
        public void OnItemChanged(Slot slot, ItemStack oldItem) { }
        
        public virtual void OnSlotClicked(AbstractSlot abstractSlot)
        {
            var slot = abstractSlot.AsInventorySlot()!;
            if (!slot.slot.stillExists) return;
            
            if (!TemporalManager.instance.IsCursorTypeOrNull(AbstractSlot.Type.INVENTORY)) return;
            var cursorSlot = TemporalManager.instance.cursorSlot?.AsInventorySlot();
            if (cursorSlot is null && slot.isEmpty) { }
            // (SWAPPING)
            if (cursorSlot is not null)
            {
                if (cursorSlot == slot)
                {
                    slot.UpdateIcon();
                    slot.ResetIconPosition();
                }
                else slot.slot.Swap(cursorSlot.slot);
                TemporalManager.instance.cursorSlot = null;
            }
            // CLICKING ON FILLED SLOT WITH EMPTY CURSOR
            else TemporalManager.instance.cursorSlot = slot;
        }
        public virtual void OnSlotHovered(AbstractSlot abstractSlot)
        {
            var manager = TemporalManager.instance;
            var slot = abstractSlot.AsInventorySlot()!;
            if (slot.isEmpty) return;
            var tooltip = manager.CreateEmptyTooltip();
            var item = slot.slot.item!.type;
            if (string.IsNullOrEmpty(item.description)) tooltip.SetText(item.name);
            else tooltip.SetText(item.name + "\n\n" + item.description);
        }
        public virtual void OnSlotUnhovered(AbstractSlot slot)
        {
            TemporalManager.instance.tooltip = null;
            InventoryManager.instance.currentSelector = null;
            InventoryManager.instance.currentOutline = null;
        }
    }
}