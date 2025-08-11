using System.Linq;
using Inventory.SlotTypes;
using Item;
using JetBrains.Annotations;

namespace Inventory
{
    public class StorageRow
    {
        public PlayerInventory.ProvidingSlot[] slots { get; private set; }
        public PlayerInventory.ProvidingSlot parent { get; }
        public StorageRow(PlayerInventory.ProvidingSlot parent) => this.parent = parent;

        [CanBeNull]
        public PlayerInventory.ProvidingSlot GetByType(SlotType type) => slots.FirstOrDefault(slot => slot.type == type);

        public void Initialize()
        {
            var extraSlots = parent.item!.type.components.Get<ExtraSlots>();
            slots = new PlayerInventory.ProvidingSlot[extraSlots.slots.Length];
            for (var i = 0; i < slots.Length; i++)
            {
                var slot = new PlayerInventory.ProvidingSlot((PlayerInventory) parent.inventory, extraSlots.slots[i]);
                slot.Parent(parent);
                slots[i] = slot;
            }
        }
        
        public void Destroy()
        {
            foreach (var slot in slots) slot.Destroy();
        }
    }
}