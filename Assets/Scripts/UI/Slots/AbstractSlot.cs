using JetBrains.Annotations;
using UI.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Slots
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class AbstractSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [CanBeNull] public SlotContent icon { get; protected set; }
        public abstract Type type { get; }
        public float size => GetComponent<RectTransform>().rect.width;
        public abstract bool isEmpty { get; }
        
        public SlotListener listener;

        public void ResetIconPosition()
        {
            if (icon != null) icon.transform.localPosition = Vector3.zero;
        }
        public abstract void UpdateIcon();
        
        public void Pickup()
        {
            if (icon is null) return;
            icon!.GetComponent<RectTransform>().sizeDelta *= new Vector2(1.2f, 1.2f);
            TemporalManager.instance.AddToDraggableLayer(icon.gameObject);
        }

        public void Clear()
        {
            if (icon !=null) icon.Destroy();
            icon = null;
        }

        public AbstractSlot InstantiateNew(SlotListener listener, Transform parent, int x, int y)
        {
            var newSlot = Instantiate(this, parent);
            newSlot.SetPosition(x, y);
            newSlot.listener = listener;
            return newSlot;
        }
        
        public void SetPosition(int x, int y)
        {
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(x*size, y*size, 0);
        }
        
        public void OnPointerClick(PointerEventData eventData) => listener?.OnSlotClicked(this);
        public void OnPointerEnter(PointerEventData eventData) => listener?.OnSlotHovered(this);
        public void OnPointerExit(PointerEventData eventData) => listener?.OnSlotUnhovered(this);
        
        [CanBeNull] public InventorySlot AsInventorySlot() => this as InventorySlot;
        [CanBeNull] public AbilitySlot AsAbilitySlot() => this as AbilitySlot;
        [CanBeNull] public ShopSlot AsShopSlot() => this as ShopSlot;

        public enum Type
        {
            INVENTORY,
            ABILITY,
            SHOP
        }
    }
}