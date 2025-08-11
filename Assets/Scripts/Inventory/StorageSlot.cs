using Inventory.SlotTypes;
using Item;
using JetBrains.Annotations;

namespace Inventory
{
    public partial class PlayerInventory
    {
        public class ProvidingSlot : Slot
        {
            [CanBeNull] public StorageRow row {get; private set;}
            public ProvidingSlot(PlayerInventory inventory, SlotType type) : base(inventory, type) { }

            public new ProvidingSlot Parent() => base.Parent() as ProvidingSlot;

            protected override void InternalSet(ItemStack newItem, bool leftoverThisItem)
            {
                base.InternalSet(newItem, leftoverThisItem);
                if (row != null)
                {
                    row.Destroy();
                    (inventory as PlayerInventory)!.storageRows.Remove(row);
                    inventory.OnRowRemoved(row);
                    row = null;
                }
                if (!(item?.type?.components?.Has<ExtraSlots>() ?? false)) return;
                row = new StorageRow(this);
                (inventory as PlayerInventory)!.storageRows.Add(row);
                row.Initialize();
                inventory.OnRowAdded(row);
            }
        }
    }
}