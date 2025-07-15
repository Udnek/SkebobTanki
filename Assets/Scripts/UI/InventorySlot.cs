using Item;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace UI
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        [CanBeNull] public GameObject icon { get; private set; }
        [CanBeNull] public ItemStack item { get; private set; }
        [CanBeNull] private Image iconImage;
        
        public bool IsEmpty => item is null;
        
        public void SetItem(ItemStack item)
        {
            if (!IsEmpty) Clear(); 
            this.item = item;
            var ic = Instantiate(InventoryManager.Instance.itemStackPrefab, transform, false);
            icon = ic;
            iconImage = ic.GetComponent<Image>();
            iconImage!.color = item!.type.Color;
            InventoryManager.Instance.AddToDraggableLayer(ic);
            Scope();
        }
        
        public void Scope()
        { 
            if (!IsEmpty) iconImage!.color = item!.type.Color;
        }
        
        public void Unscope()
        {
            if (!IsEmpty) iconImage!.color = iconImage.color * new Color(1f, 1f, 1f, 0.5f);
        }

        public void Clear()
        {
            if (IsEmpty) return;
            Destroy(icon);
            icon = null;
            iconImage = null;
            item = null;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            InventoryManager.Instance.OnSlotClicked(this);
        }
    }
}