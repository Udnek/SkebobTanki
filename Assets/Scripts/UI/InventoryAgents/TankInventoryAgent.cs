#nullable enable
using System.Collections.Generic;
using Inventory;
using Item;
using Item.Components;
using UI.Managers;
using UI.Slots;
using UnityEngine;

namespace UI.InventoryAgents
{
    public class TankInventoryAgent : AbstractInventoryAgent<PlayerInventory>, InventoryListener
    {
        private readonly Dictionary<StorageRow, SlotContent?> rowToIcon = new();
        
        protected override PlayerInventory inventory { get; set; }
        public TankInventoryAgent(PlayerInventory inventory) => this.inventory = inventory;

        private void CreateSlot(StorageRow? row, Slot slot, int x, int y, Transform layer, bool doAnimation)
        {
            var createdSlot = InventoryManager.instance.slotPrefab.InstantiateNew(this, layer, x, y).AsInventorySlot()!;
            createdSlot.slot = slot;
            //var createdSlot = InventoryManager.instance.CreateSlot(slot, x, y, layer);
            slots[slot] = createdSlot;
            if (!doAnimation) return;
            createdSlot.transform.localScale = new Vector3();
            slotsToAppear.GetOrAdd(row!.parent, new List<InventorySlot>()).Add(createdSlot);
        }
        private void CreateStorageSlot(StorageRow row, Slot slot, int x, int y, bool doAnimation = true)
            => CreateSlot(row, slot, x, y, InventoryManager.instance.storageSlotsLayer, doAnimation);
        private void CreateHullSlot(StorageRow? row, Slot slot, int x, int y, bool doAnimation = true)
            => CreateSlot(row, slot, x, y, InventoryManager.instance.hullSlotsLayer, doAnimation);
        private void CreateIcon(StorageRow row, int x, int y, ItemType type)
        {
            var icon = Object.Instantiate(InventoryManager.instance.itemStackPrefab, InventoryManager.instance.storageSlotsLayer);
            icon.transform.localPosition += new Vector3(x*InventoryManager.instance.slotPrefab.size, y*InventoryManager.instance.slotPrefab.size, 0);
            rowToIcon[row] = icon;
            icon.image.sprite = type.icon;
        }
        
        public override void Open()
        {
            base.Open();
            var manager = InventoryManager.instance;
            
            // HULL
            CreateHullSlot(null, inventory.hullSlot, 0, 0, false);
            if (inventory.hullSlot.storage != null)
            {
                for (var index = 0; index < inventory.hullSlot.storage.slots.Length; index++) 
                    CreateHullSlot(inventory.hullSlot.storage, inventory.hullSlot.storage.slots[index], 0, -index-1, false);
            }
            
            // STORAGE
            var y = 0;
            foreach (var row in inventory!.storageRows)
            {
                if (row.parent.type == SlotType.HULL) continue;
                CreateIcon(row, 0, y, row!.parent!.item!.type);
                for (var slotIndex = 0; slotIndex < row.slots.Length; slotIndex++) 
                    CreateStorageSlot(row, row.slots[slotIndex], slotIndex + 1, y, false);
                y--;
            }

            // BACKPACK
            for (var index = 0; index < inventory.backpack.Length; index++)
            {
                var slot = inventory.backpack[index];
                CreateSlot(null, slot, index, 0, manager.backpackSlotsLayer, false);
            }
        }
        public override void OnRowRemoved(StorageRow toRemoveRow)
        {
            foreach (var slot in toRemoveRow.slots)
            {
                Object.Destroy(slots.GetAndRemove(slot).gameObject);
            }
            if (toRemoveRow.parent.type == SlotType.HULL) return;
            
            rowToIcon.GetAndRemove(toRemoveRow)!.Destroy();
            
            var y = 0;
            foreach (var row in inventory.storageRows)
            {
                if (row.parent.type == SlotType.HULL) continue;
                float yPos =  y * InventoryManager.instance.slotPrefab.size;
                
                var icon = rowToIcon[row]!;
                var iconPos = icon.transform.localPosition;
                iconPos.y = yPos;
                SmoothMover.RunLocal(icon.gameObject, 1, iconPos);
                
                foreach (var slot in row.slots)
                {
                    var visualSlot = slots[slot];
                    var pos = visualSlot.transform.localPosition;
                    pos.y = yPos;
                    SmoothMover.RunLocal(visualSlot.gameObject, 1, pos);
                }
                y--;
            }
        }
        public override void OnRowAdded(StorageRow row)
        {
            if (row.parent.type == SlotType.HULL)
            {
                for (var i = 0; i < row.slots.Length; i++) CreateHullSlot(row, row.slots[i], 0, -i-1);
            }
            else
            {
                int y = -inventory.storageRows.Count+2;
                CreateIcon(row, 0, y, row!.parent!.item!.type);
                for (var i = 0; i < row.slots.Length; i++) CreateStorageSlot(row, row.slots[i], i+1, y);
            }
        }

        public override void OnSlotHovered(AbstractSlot abstractSlot)
        {
            base.OnSlotHovered(abstractSlot);
            var slot = abstractSlot.AsInventorySlot()!;
            CreateSelector(slot);
            CreateOutline(slot);
        }

        private void CreateSelector(InventorySlot slot)
        {
            if (slot.slot is not PlayerInventory.StorageSlot storageSlot) return;
            if (storageSlot.storage == null) return;
            var icon = rowToIcon.GetValueOrDefault(storageSlot.storage);
            if (icon is null) return;
            InventoryManager.instance.selectorPrefab.InstantiateNew().transform.position = icon.transform.position;
        }
        
        private void CreateOutline(InventorySlot slot)
        {
            var part = slot.slot?.item?.components?.Get<InitiatedPart>()?.script?.modelToOutline;
            if (part is null) return;
            var outline = part.AddComponent<Outline>();
            InventoryManager.instance.currentOutline = outline;
            outline.OutlineWidth = 10;
            outline.OutlineColor = new Color(96, 208, 255, 255) / 255f;
        }
    }
}












