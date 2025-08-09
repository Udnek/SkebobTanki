using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UI.InventoryAgents;
using UI.Slots;
using UnityEngine;

namespace UI.Managers
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager instance { get; private set; }
        [field: SerializeField] public SlotContent itemStackPrefab { get; private set; }
        [field: SerializeField] public Transform hullSlotsLayer {get; private set;}
        [field: SerializeField] public Transform storageSlotsLayer {get; private set;}
        [field: SerializeField] public Transform backpackSlotsLayer {get; private set;}
        [field: SerializeField] public InventorySlot slotPrefab {get; private set;}
        [field: SerializeField] public Selector selectorPrefab { get; private set; }

        private InventoryAgent[] inventoryRenderers = Array.Empty<InventoryAgent>();
        
        [CanBeNull] private Selector currentSelectorField;
        [CanBeNull]
        public Selector currentSelector
        {
            get => currentSelectorField;
            set
            {
                currentSelectorField?.Destroy();
                currentSelectorField = value;
            }
        }
        [CanBeNull] private Outline currentOutlineField;
        [CanBeNull]
        public Outline currentOutline
        {
            get => currentOutlineField;
            set
            {
                if (currentOutlineField != null) Destroy(currentOutlineField);
                currentOutlineField = value;
            }
        }
        
        private void Awake() => instance = this;
        [CanBeNull] public InventorySlot GetSlot(Inventory.Slot slot)
        {
            return inventoryRenderers
                .Select(renderer => renderer.GetSlot(slot))
                .FirstOrDefault(s => s is not null);
        }

        public void Open(params InventoryAgent[] renderers)
        {
            inventoryRenderers = renderers;
            foreach (var render in renderers) render.Open();
        }

        public void Close()
        {
            foreach (var renderer in inventoryRenderers) renderer.Close();
            inventoryRenderers = Array.Empty<InventoryAgent>();
            currentSelector = null;
            currentOutline = null;
        }
    }
}