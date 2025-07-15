using System;
using Item;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }
        [field: SerializeField] public GameObject itemStackPrefab{get; private set; }
        
        [SerializeField] private GameObject draggableLayer;
        [SerializeField] private GameObject UIInventoryObject;
        [SerializeField] private InventorySlot[] shopSlots;
        [SerializeField] private InventorySlot[] currentSlots;
        [CanBeNull] private InventorySlot currentlyMovingFromSlot;
        
        public bool isOpened { get; private set; } = true;
        
        private InventoryManager() {}
        private void Awake() => Instance = this;

        private void Start() => Close();

        public void AddToDraggableLayer(GameObject go) => go.transform.SetParent(draggableLayer.transform);
        
        public void OnSlotClicked(InventorySlot slot)
        {
            // CLICKING ON EMPTY SLOT WITH EMPTY CURSOR
            if (currentlyMovingFromSlot == null && slot.IsEmpty) {}
            // CLICKING ON EMPTY SLOT WITH FILLED CURSOR
            else if (currentlyMovingFromSlot != null && slot.IsEmpty)
            {
                slot.SetItem(currentlyMovingFromSlot.item);
                currentlyMovingFromSlot.Clear();
                currentlyMovingFromSlot = null;
            }
            // CLICKING ON FILLED SLOT WITH FILLED CURSOR (SWAPPING)
            else if (currentlyMovingFromSlot != null && !slot.IsEmpty)
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
                slot.Unscope();
            }
        }

        private void Update()
        {
            if (currentlyMovingFromSlot is null) return;
            if (currentlyMovingFromSlot.IsEmpty) return;
            currentlyMovingFromSlot.icon!.transform.position = Input.mousePosition;
        }

        public void Open(Inventory inventory)
        {
            if (isOpened) return;
            isOpened = true;
            UIInventoryObject.SetActive(true);

            var items = inventory.GetItems();
            for (var i = 0; i < Math.Min(items.Length, currentSlots.Length); i++)
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
            UIInventoryObject.SetActive(false);
        }

        public void Toggle(Inventory inventory)
        {
            if (isOpened) Close();
            else Open(inventory);
        }
    }
}