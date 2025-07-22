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
            inventory.listener = this;
        }

        public override void Open()
        {
            var manager = InventoryManager.instance;
            for (var rowIndex = 0; rowIndex < inventory!.rows.Length; rowIndex++)
            {
                var row = inventory.rows[rowIndex];
                
                slots.Add(row.main, manager.CreateSlot(row.main, 0, rowIndex, manager.playerSlotsLayer));
                for (var extraIndex = 0; extraIndex < row.extra.Length; extraIndex++)
                {
                    var slot = row.extra[extraIndex];
                    slots.Add(slot, manager.CreateSlot(slot, extraIndex + 1, rowIndex, manager.playerSlotsLayer));
                }
            }

            for (var index = 0; index < inventory.backpack.Length; index++)
            {
                var slot = inventory.backpack[index];
                slots.Add(slot, manager.CreateSlot(slot, index, 0, manager.backpackSlotsLayer));
            }
        }

        public override void Close()
        {
            inventory.listener = null;
            base.Close();
        }
        
        public override void OnSlotAdded(Inventory.Slot slot, Row row)
        {
            var manager = InventoryManager.instance;
            for (var i = 0; i < inventory!.rows.Length; i++)
            {
                var currentRow = inventory.rows[i];
                if (currentRow != row) continue;
                for (var j = 0; j < currentRow.extra.Length; j++)
                {
                    if (currentRow.extra[j] != slot) continue;
                    var createdSlot = manager.CreateSlot(slot, j + 1, i, manager.playerSlotsLayer);
                    createdSlot.transform.localScale = new Vector3();
                    slots.Add(slot, createdSlot);
                    slotsToAppear.GetOrAdd(currentRow.main, new List<Slot>()).Add(createdSlot);
                    return;
                }
            }
        }
    }
}