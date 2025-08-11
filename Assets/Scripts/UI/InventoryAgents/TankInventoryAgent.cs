#nullable enable
using System.Collections.Generic;
using Inventory;
using Inventory.SlotTypes;
using Item;
using UI.Managers;
using UI.Slots;
using UnityEngine;

namespace UI.InventoryAgents
{
    public class TankInventoryAgent : AbstractInventoryAgent<PlayerInventory>, InventoryListener
    {
        private readonly Dictionary<StorageRow, ProviderSlot?> rowToProvider = new();
        protected sealed override PlayerInventory inventory { get; set; }
        public TankInventoryAgent(PlayerInventory inventory) => this.inventory = inventory;

        private void CreateSlot(Slot? slotToWait, Slot slot, int x, int y, Transform layer)
        {
            var createdSlot = InventoryManager.instance.slotPrefab.InstantiateNew(this, layer, x, y).AsInventorySlot()!;
            createdSlot.slot = slot;
            slots[slot] = createdSlot;
            if (slotToWait is null) return;
            createdSlot.transform.localScale = new Vector3();
            slotsToAppear.GetOrAdd(slotToWait, new List<InventorySlot>()).Add(createdSlot);
        }
        private void CreateStorageSlot(PlayerInventory.ProvidingSlot? slotToWait, PlayerInventory.ProvidingSlot slot, int x, int y)
            => CreateSlot(slotToWait, slot, x, y, InventoryManager.instance.storageSlotsLayer);
        private void CreateHullSlot(PlayerInventory.ProvidingSlot? hull, PlayerInventory.ProvidingSlot slot, int x, int y)
            => CreateSlot(hull, slot, x, y, InventoryManager.instance.hullSlotsLayer);
        private void CreateProvider(StorageRow row, int x, int y, Slot slot, bool animate)
        {
            var provider = InventoryManager.instance.slotProviderPrefab.InstantiateNew(this, InventoryManager.instance.storageSlotsLayer, x, y).AsProviderSlot()!;
            provider.slot = slot;
            rowToProvider[row] = provider;
            if (!animate) return;
            provider.transform.localScale = new Vector3();
            slotsToAppear.GetOrAdd(row.parent, new List<InventorySlot>()).Add(provider);
        }
        
        public override void Open()
        {
            base.Open();
            var manager = InventoryManager.instance;
            
            // HULL
            CreateHullSlot(null, inventory.hullSlot, 0, 0);
            if (inventory.hullSlot.row != null)
            {
                for (var index = 0; index < inventory.hullSlot.row.slots.Length; index++) 
                    CreateHullSlot(null, inventory.hullSlot.row.slots[index], 0, -index-1);
            }
            
            // STORAGE
            var y = 0;
            foreach (var row in inventory.storageRows)
            {
                if (row.parent.type.isHull) continue;
                CreateProvider(row, 0, y, row.parent!, false);
                for (var slotIndex = 0; slotIndex < row.slots.Length; slotIndex++) 
                    CreateStorageSlot(null, row.slots[slotIndex], slotIndex + 1, y);
                y--;
            }

            // BACKPACK
            for (var index = 0; index < inventory.backpack.Length; index++)
            {
                var slot = inventory.backpack[index];
                CreateSlot(null, slot, index, 0, manager.backpackSlotsLayer);
            }
        }
        public override void OnRowRemoved(StorageRow toRemoveRow)
        {
            foreach (var slot in toRemoveRow.slots)
            {
                Object.Destroy(slots.GetAndRemove(slot).gameObject);
            }
            if (toRemoveRow.parent.type.isHull) return;
            
            Object.Destroy(rowToProvider.GetAndRemove(toRemoveRow)!.gameObject);
            
            var y = 0;
            foreach (var row in inventory.storageRows)
            {
                if (row.parent.type.isHull) continue;
                float yPos =  y * InventoryManager.instance.slotPrefab.size;
                
                var provider = rowToProvider[row]!;
                var iconPos = provider.transform.localPosition;
                iconPos.y = yPos;
                SmoothMover.RunLocal(provider.gameObject, 1, iconPos);
                
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
            if (row.parent.type.isHull)
            {
                for (var i = 0; i < row.slots.Length; i++) CreateHullSlot(row.parent, row.slots[i], 0, -i-1);
            }
            else
            {
                int y = -inventory.storageRows.Count+2;
                CreateProvider(row, 0, y, row.parent, true);
                for (var i = 0; i < row.slots.Length; i++) CreateStorageSlot(row.parent, row.slots[i], i+1, y);
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
            if (slot.type == AbstractSlot.Type.PROVIDER)
            {
                var real = InventoryManager.instance.GetSlot(slot.slot)!;
                InventoryManager.instance.selectorPrefab.InstantiateNew(real.transform).transform.position = real.transform.position;
            }
            else
            {
                if (slot.slot is not PlayerInventory.ProvidingSlot storageSlot) return;
                if (storageSlot.row == null) return;
                var provider = rowToProvider.GetValueOrDefault(storageSlot.row);
                if (provider is null) return;
                InventoryManager.instance.selectorPrefab.InstantiateNew(slot.transform).transform.position = provider.transform.position;
            }

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












