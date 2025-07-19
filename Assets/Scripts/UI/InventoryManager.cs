using System;
using System.Collections.Generic;
using Item;
using JetBrains.Annotations;
using UnityEngine;

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
        [SerializeField] private GameObject shopSlotsLayer;
        [SerializeField] private GameObject currentSlotsLayer;

        private List<InventorySlot> shopSlots { get; } = new();
        private List<InventorySlot> currentSlots { get; } = new();
        
        [CanBeNull] private InventorySlot currentlyMovingFromSlot;
        
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
        
        private InventoryManager() {}
        
        private void Awake() => instance = this;

        private void Start()
        {
            var prefabWidth = slotPrefab.GetComponent<RectTransform>().rect.width;
            Run(shopSlotsLayer,  shopSlots, 5);
            Run(currentSlotsLayer,  currentSlots, Inventory.SIZE);

            Close();
            return;

            void Run(GameObject layer, List<InventorySlot> slots, int amount)
            {
                for (int i = 0; i < amount; i++)
                {
                    var go = Instantiate(slotPrefab, layer.transform);
                    go.GetComponent<RectTransform>().localPosition = new Vector3(i * prefabWidth, 0, 0);
                    slots.Add(go.GetComponent<InventorySlot>()!);
                }
            }
        }

        private void AddToDraggableLayer(GameObject go) => go.transform.SetParent(draggableLayer);
        
        public void OnSlotClicked(InventorySlot slot)
        {
            // CLICKING ON EMPTY SLOT WITH EMPTY CURSOR
            if (currentlyMovingFromSlot is null && slot.isEmpty) {}
            // CLICKING ON EMPTY SLOT WITH FILLED CURSOR
            else if (currentlyMovingFromSlot is not null && slot.isEmpty)
            {
                slot.SetItem(currentlyMovingFromSlot.item);
                currentlyMovingFromSlot.Clear();
                currentlyMovingFromSlot = null;
            }
            // CLICKING ON FILLED SLOT WITH FILLED CURSOR (SWAPPING)
            else if (currentlyMovingFromSlot is not null && !slot.isEmpty)
            {
                var cache = currentlyMovingFromSlot.item;
                currentlyMovingFromSlot.SetItem(slot.item);
                slot.SetItem(cache);
                currentlyMovingFromSlot = null;
            }
            // CLICKING ON FILLED SLOT WITH EMPTY CURSOR
            else
            {
                currentlyMovingFromSlot = slot;
                AddToDraggableLayer(slot.icon!);
                slot.Pickup();
            }
        }
        
        public void OnSlotHovered(InventorySlot slot)
        {
            if (slot.isEmpty) return;
            if (tooltip is not null) Destroy(tooltip);
            tooltip = Instantiate(UIManager.instance.tooltipPrefab, tooltipLayer, false).GetComponent<Tooltip>();
            var item = slot.item!.type;
            tooltip!.SetText(item.name + "\n\n" + item.description);
        }

        public void OnSlotUnhovered(InventorySlot slot) => tooltip = null;

        private void Update()
        {
            if (currentlyMovingFromSlot is null) return;
            if (currentlyMovingFromSlot.isEmpty) return;
            currentlyMovingFromSlot.icon!.transform.position = Input.mousePosition;
        }

        public void Open(Inventory inventory)
        {
            if (isOpened) return;
            isOpened = true;
            uiInventoryObject.SetActive(true);

            var items = inventory.GetItems();
            for (var i = 0; i < Math.Min(items.Length, currentSlots.Count); i++)
            {
                var item = items[i];
                var slot = currentSlots[i];
                if (item is null) continue;
                
                slot.SetItem(item);
            }
        }

        public void Close()
        {
            if (!isOpened) return;
            isOpened = false;
            foreach (var slot in shopSlots) slot.Clear();
            foreach (var slot in currentSlots) slot.Clear();
            tooltip = null;
            uiInventoryObject.SetActive(false);
        }

        public void Toggle(Inventory inventory)
        {
            if (isOpened) Close();
            else Open(inventory);
        }
    }
}