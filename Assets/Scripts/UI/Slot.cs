using Inventory;
using Item;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [CanBeNull] private GameObject iconField;
        [CanBeNull] public GameObject icon
        {
            get => iconField;
            private set
            {
                iconField = value;
                iconImage = iconField?.GetComponent<Image>();
            }
        }

        [CanBeNull] private Image iconImage;
        
        private Inventory.Slot slotField;
        public Inventory.Slot slot
        {
            get => slotField;
            set
            {
                slotField = value;
                gameObject.name = slot.type.name;
                UpdateIcon();
            }
        }
        public bool isEmpty => slot.item is null;
        
        public void UpdateIcon()
        {
            var pos = icon?.transform.position;
            Clear(); 
            if (slot.item is null) return;
            var ic = Instantiate(InventoryManager.instance.itemStackPrefab, transform, false);
            if (pos != null) {ic.transform.position = (Vector3) pos;}
            icon = ic;
            iconImage!.sprite = slot.item.type.icon;
        }
        
        public void Pickup()
        {
            if (icon is null) return;
            icon!.GetComponent<RectTransform>().sizeDelta *= new Vector2(1.2f, 1.2f);
        }

        public void Clear()
        {
            if (icon is not null) Destroy(icon);
            icon = null;
        }
        
        public void OnPointerClick(PointerEventData eventData) => InventoryManager.instance.OnSlotClicked(this);
        public void OnPointerEnter(PointerEventData eventData) => InventoryManager.instance.OnSlotHovered(this);
        public void OnPointerExit(PointerEventData eventData) => InventoryManager.instance.OnSlotUnhovered(this);
    }
}