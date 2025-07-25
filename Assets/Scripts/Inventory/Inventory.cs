using ECS;
using Item;
using JetBrains.Annotations;

namespace Inventory
{
    public abstract class Inventory : InventoryListener, CustomComponent<Tank.Tank>
    {
        public delegate void ItemSet(Slot slot, [CanBeNull] ItemStack oldItem);
        public delegate void ItemSwapped(Slot thisSlot, Slot withSlot);
        public delegate void SlotRemoved(Slot slot, Row row);
        public delegate void SlotAdded(Slot slot, Row row);
        public delegate void ItemChanged(Slot slot, [CanBeNull] ItemStack oldItem);
        
        public event ItemSet onItemSet;
        public event ItemSwapped onItemSwapped;
        public event SlotRemoved onSlotRemoved;
        public event SlotAdded onSlotAdded;
        public event ItemChanged onItemChanged;
        
        public virtual void OnItemSet(Slot slot, [CanBeNull] ItemStack oldItem)
        {
            onItemSet?.Invoke(slot, oldItem);
            onItemChanged?.Invoke(slot, oldItem);
        }

        public virtual void OnItemSwapped(Slot thisSlot, Slot withSlot)
        {
            onItemSwapped?.Invoke(thisSlot, withSlot);
            onItemChanged?.Invoke(thisSlot, withSlot.item);
        }

        public virtual void OnSlotRemoved(Slot slot, Row row) => onSlotRemoved?.Invoke(slot, row);
        public virtual void OnSlotAdded(Slot slot, Row row) => onSlotAdded?.Invoke(slot, row);
        public void OnItemChanged(Slot slot, ItemStack oldItem) { }

        public void Subscribe(InventoryListener listener)
        {
            onItemSet += listener.OnItemSet;
            onItemSwapped += listener.OnItemSwapped;
            onSlotAdded += listener.OnSlotAdded;
            onSlotRemoved += listener.OnSlotRemoved;
            onItemChanged += listener.OnItemChanged;
        }

        public void Unsubscribe(InventoryListener listener)
        {
            onItemSet -= listener.OnItemSet;
            onItemSwapped -= listener.OnItemSwapped;
            onSlotAdded -= listener.OnSlotAdded;
            onSlotRemoved -= listener.OnSlotRemoved;
            onItemChanged -= listener.OnItemChanged;
        }

        public abstract void TakeLeftover(Slot slot);
    }
}