using Inventory;
using UI.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Slots
{
    public class InventorySlot : AbstractSlot
    {
        public override Type type => Type.INVENTORY;

        private SlotContent slotTypeIcon;
        
        protected Slot slotField;
        public virtual Slot slot
        {
            get => slotField;
            set
            {
                slotField = value;
                gameObject.name = slot.type.name;
                UpdateIcon();
            }
        }
        public override bool isEmpty => slot.item is null;

        public override void UpdateIcon()
        {
            if (slot.item is null)
            {
                Clear();
                SetSlotIcon();
                return;
            }
            RemoveSlotIcon();
            if (icon is null) icon = Instantiate(InventoryManager.instance.itemStackPrefab, transform);
            else icon.transform.SetParent(transform);
            icon!.image.sprite = slot.item.type.icon;
            icon.transform.localScale = Vector3.one;
        }

        private void RemoveSlotIcon()
        {
            slotTypeIcon?.Destroy();
            slotTypeIcon = null;
        }
        
        private void SetSlotIcon()
        {
            if (slotTypeIcon != null) return;
            if (slot.type.icon == null) return;
            slotTypeIcon = Instantiate(InventoryManager.instance.slotTypePrefab, transform);
            slotTypeIcon.image.sprite = slot.type.icon;
        }
        
        public override void PutBack()
        {
            base.PutBack();
            RemoveSlotIcon();
        }

        public override void Pickup()
        {
            base.Pickup();
            SetSlotIcon();
        }

        public override void Clear()
        {
            base.Clear();
            RemoveSlotIcon();
        }
    }
}












