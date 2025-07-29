using Item;

namespace Inventory
{
    public class StorageRow
    {
        public Slot[] slots { get; private set; }
        public PlayerInventory.StorageSlot parent { get; private set; }
        internal StorageRow(PlayerInventory.StorageSlot parent) => this.parent = parent;

        //public bool isEmpty => slots.All(slot => slot.item == null);
        
        public void Initialize()
        {
            var extraSlots = parent.item!.type.components.Get<ExtraSlots>();
            slots = new Slot[extraSlots.slots.Length];
            for (var i = 0; i < slots.Length; i++)
            {
                var slot = new PlayerInventory.StorageSlot((PlayerInventory) parent.inventory, extraSlots.slots[i]);
                slot.parent = parent;
                slots[i] = slot;
            }
        }
        
        public void Destroy()
        {
            foreach (var slot in slots) slot.Destroy();
        }
    }
}