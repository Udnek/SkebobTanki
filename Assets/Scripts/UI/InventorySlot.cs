using Item;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [CanBeNull] public GameObject icon { get; private set; }
        [CanBeNull] public ItemStack item { get; private set; }
        [CanBeNull] private Image iconImage;
        
        public bool isEmpty => item is null;
        
        public void SetItem(ItemStack item)
        {
            if (!isEmpty) Clear(); 
            this.item = item;
            var ic = Instantiate(InventoryManager.instance.itemStackPrefab, transform, false);
            icon = ic;
            iconImage = ic.GetComponent<Image>();
            iconImage!.sprite = item.type.icon;
        }
        
        public void Pickup()
        {
            if (isEmpty) return;
            icon!.GetComponent<RectTransform>().sizeDelta *= new Vector2(1.2f, 1.2f);
        }

        public void Clear()
        {
            if (isEmpty) return;
            Destroy(icon);
            icon = null;
            iconImage = null;
            item = null;
        }
        
        public void OnPointerClick(PointerEventData eventData) => InventoryManager.instance.OnSlotClicked(this);
        public void OnPointerEnter(PointerEventData eventData) => InventoryManager.instance.OnSlotHovered(this);
        public void OnPointerExit(PointerEventData eventData) => InventoryManager.instance.OnSlotUnhovered(this);
    }
}