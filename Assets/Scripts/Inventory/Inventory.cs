using ECS;
using Item;
using JetBrains.Annotations;

namespace Inventory
{
    public abstract class Inventory : InventoryListener, CustomComponent<Tank.Tank>
    {
        public delegate void ItemSet(Slot slot, [CanBeNull] ItemStack oldItem);
        public delegate void ItemSwapped(Slot thisSlot, Slot withSlot);
        public delegate void RowRemoved(StorageRow row);
        public delegate void RowAdded(StorageRow row);
        public delegate void ItemChanged(Slot slot, [CanBeNull] ItemStack oldItem);
        
        public event ItemSet onItemSet;
        public event ItemSwapped onItemSwapped;
        public event RowRemoved onRowRemoved;
        public event RowAdded onRowAdded;
        public event ItemChanged onItemChanged;
        
        public virtual void OnItemSet(Slot slot, ItemStack oldItem)
        {
            onItemSet?.Invoke(slot, oldItem);
            onItemChanged?.Invoke(slot, oldItem);
        }

        public virtual void OnItemSwapped(Slot thisSlot, Slot withSlot)
        {
            onItemSwapped?.Invoke(thisSlot, withSlot);
            onItemChanged?.Invoke(thisSlot, withSlot.item);
        }

        public virtual void OnRowRemoved(StorageRow row) => onRowRemoved?.Invoke(row);
        public virtual void OnRowAdded(StorageRow row) => onRowAdded?.Invoke(row);
        public void OnItemChanged(Slot slot, ItemStack oldItem) { }

        public void Subscribe(InventoryListener listener)
        {
            onItemSet += listener.OnItemSet;
            onItemSwapped += listener.OnItemSwapped;
            onRowAdded += listener.OnRowAdded;
            onRowRemoved += listener.OnRowRemoved;
            onItemChanged += listener.OnItemChanged;
        }

        public void Unsubscribe(InventoryListener listener)
        {
            onItemSet -= listener.OnItemSet;
            onItemSwapped -= listener.OnItemSwapped;
            onRowAdded -= listener.OnRowAdded;
            onRowRemoved -= listener.OnRowRemoved;
            onItemChanged -= listener.OnItemChanged;
        }

        public abstract void TakeLeftover(Slot slot);
    }
}