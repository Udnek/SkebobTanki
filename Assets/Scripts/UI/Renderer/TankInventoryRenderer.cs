using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace UI.Renderer
{
    public class TankInventoryRenderer : AbstractInventoryRenderer<PlayerInventory>, InventoryListener
    {
        protected override PlayerInventory inventory { get; set; }
        
        public TankInventoryRenderer(PlayerInventory inventory)
        {
            this.inventory = inventory;
        }

        public override void Open()
        {
            base.Open();
            var manager = InventoryManager.instance;
            
            slots.Add(inventory.hullSlot, manager.CreateSlot(inventory.hullSlot, 0, 0, manager.playerSlotsLayer));
            
            for (var rowIndex = 0; rowIndex < inventory!.storageRows.Count; rowIndex++)
            {
                var row = inventory.storageRows[rowIndex];
                for (var slotIndex = 0; slotIndex < row.slots.Length; slotIndex++)
                {
                    var slot = row.slots[slotIndex];
                    slots.Add(slot, manager.CreateSlot(slot, slotIndex + 1, -rowIndex, manager.playerSlotsLayer));
                }
            }

            for (var index = 0; index < inventory.backpack.Length; index++)
            {
                var slot = inventory.backpack[index];
                slots.Add(slot, manager.CreateSlot(slot, index, 0, manager.backpackSlotsLayer));
            }
        }

        public override void OnRowRemoved(StorageRow toRemoveRow)
        {
            foreach (var slot in toRemoveRow.slots)
            {
                Object.Destroy(slots[slot].gameObject);
                slots.Remove(slot);
            }

            for (var rowI = 0; rowI < inventory.storageRows.Count; rowI++)
            {
                var row = inventory.storageRows[rowI];
                foreach (var slot in row.slots)
                {
                    var visualSlot = slots[slot];
                    var pos = visualSlot.transform.localPosition;
                    pos.y = -rowI * InventoryManager.instance.slotSize;
                    SmoothMover.RunLocal(visualSlot.gameObject, 1, pos);
                }
            }
        }

        public override void OnRowAdded(StorageRow row)
        {
            var manager = InventoryManager.instance;
            int y = -inventory.storageRows.Count+1;
            for (var i = 0; i < row.slots.Length; i++)
            {
                var slot = row.slots[i];
                var createdSlot = manager.CreateSlot(slot, i + 1, y, manager.playerSlotsLayer);
                createdSlot.transform.localScale = new Vector3();
                slots.Add(slot, createdSlot);
                slotsToAppear.GetOrAdd(row.parent, new List<Slot>()).Add(createdSlot);
            }
        }
    }
}