using Item;
using Item.Components;
using JetBrains.Annotations;

namespace Inventory
{
    public partial class PlayerInventory
    {
        public class StorageSlot : Slot
        {
            [CanBeNull] public StorageRow storage {get; private set;}
            public StorageSlot(PlayerInventory inventory, SlotType type) : base(inventory, type) { }
            
            protected override void InternalSet(ItemStack newItem, bool leftoverThisItem)
            {
                base.InternalSet(newItem, leftoverThisItem);
                if (storage != null)
                {
                    storage.Destroy();
                    (inventory as PlayerInventory)!.storageRows.Remove(storage);
                    inventory.OnRowRemoved(storage);
                    storage = null;
                }
                if (!(item?.type?.components?.Has<ExtraSlots>() ?? false)) return;
                storage = new StorageRow(this);
                (inventory as PlayerInventory)!.storageRows.Add(storage);
                storage.Initialize();
                inventory.OnRowAdded(storage);
            }
        }
    }
}