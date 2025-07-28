using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using Item;
using JetBrains.Annotations;
using NUnit.Framework;
using UI.Renderer;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace UI
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager instance { get; private set; }
        
        [field: SerializeField] public GameObject itemStackPrefab { get; private set; }
        [field: SerializeField] public GameObject slotPrefab { get; private set; }
        
        [SerializeField] private Transform draggableLayer;
        [SerializeField] private GameObject uiInventoryObject;
        [SerializeField] private Transform tooltipLayer;
        
        [field: SerializeField] public Transform shopSlotsLayer {get; private set;}
        [field: SerializeField] public Transform playerSlotsLayer {get; private set;}
        [field: SerializeField] public Transform backpackSlotsLayer {get; private set;}

        private float? slotSizeField;
        public float slotSize
        {
            get
            {
                slotSizeField ??= slotPrefab.GetComponent<RectTransform>().rect.width;
                return (float) slotSizeField;
            }
        }

        private InventoryRenderer[] inventoryRenderers = Array.Empty<InventoryRenderer>();
        [CanBeNull] public Slot currentlyMovingSlot { get; private set; }
        [CanBeNull] private Tooltip tooltipField;
        

        [CanBeNull] public Tooltip tooltip
        {
            get => tooltipField;
            set
            {
                if (tooltipField is not null) Destroy(tooltipField.gameObject);
                tooltipField = value;
            }
        }

        public bool isOpened { get; private set; } = true;

        [CanBeNull] public Slot GetSlot(Inventory.Slot slot)
        {
            return inventoryRenderers
                .Select(renderer => renderer.GetSlot(slot))
                .FirstOrDefault(s => s is not null);
        }
        
        public void AddToDraggableLayer(GameObject go) => go.transform.SetParent(draggableLayer);
        
        public void OnSlotClicked(Slot slot)
        {
            if (!slot.slot.stillExists) return;
            // CLICKING ON EMPTY SLOT WITH EMPTY CURSOR
            if (currentlyMovingSlot is null && slot.isEmpty) {}
            // (SWAPPING)
            else if (currentlyMovingSlot is not null)
            {
                if (currentlyMovingSlot == slot)
                {
                    slot.UpdateIcon();
                    slot.ResetIconPosition();
                }
                else slot.slot.Swap(currentlyMovingSlot.slot);
                currentlyMovingSlot = null;
            }
            // CLICKING ON FILLED SLOT WITH EMPTY CURSOR
            else
            {
                currentlyMovingSlot = slot;
                AddToDraggableLayer(slot.icon!);
                slot.Pickup();
            }
        }
        
        public void OnSlotHovered(Slot slot)
        {
            if (slot.isEmpty) return;
            if (tooltip is not null) Destroy(tooltip);
            tooltip = Instantiate(UIManager.instance.tooltipPrefab, tooltipLayer, false).GetComponent<Tooltip>();
            var item = slot.slot.item!.type;
            if (string.IsNullOrEmpty(item.description)) tooltip!.SetText(item.name);
            else tooltip!.SetText(item.name + "\n\n" + item.description);
        }

        public void OnSlotUnhovered(Slot slot) => tooltip = null;
        
        private void Awake() => instance = this;

        private void Start()
        {
            isOpened = true;
            Close();
        }
        
        private void Update()
        {
            if (currentlyMovingSlot is null) return;
            if (currentlyMovingSlot.isEmpty) return;
            currentlyMovingSlot.icon!.transform.position = Input.mousePosition;
        }
        
        public Slot CreateSlot(Inventory.Slot realSlot, int x, int y, Transform layer)
        {
            var go = Instantiate(slotPrefab, layer.transform);
            var rectTransform = go.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(x*slotSize, y*slotSize, 0);
            var slot = go.GetComponent<Slot>()!;
            slot.slot = realSlot;
            return slot;
        }

        public void Open(params InventoryRenderer[] renderers)
        {
            if (isOpened) return;
            isOpened = true;
            inventoryRenderers = renderers;
            foreach (var render in renderers) render.Open();
            uiInventoryObject.SetActive(true);
        }

        public void Close()
        {
            if (!isOpened) return;
            foreach (var renderer in inventoryRenderers) renderer.Close();
            inventoryRenderers = Array.Empty<InventoryRenderer>();
            isOpened = false;
            tooltip = null;
            uiInventoryObject.SetActive(false);
        }

        public void Toggle(params InventoryRenderer[] renderers)
        {
            if (isOpened) Close();
            else Open(renderers);
        }
    }
}