using UI.Managers;
using UnityEngine;

namespace UI.Slots
{
    public class InventorySlot : AbstractSlot
    {
        public override Type type => Type.INVENTORY;
        
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
        public override bool isEmpty => slot.item is null;

        public override void UpdateIcon()
        {
            var pos = icon?.transform.position;
            Clear(); 
            if (slot.item is null) return;
            var ic = Instantiate(InventoryManager.instance.itemStackPrefab, transform, false);
            if (pos != null) ic.transform.position = (Vector3) pos;
            icon = ic;
            ic.image.sprite = slot.item.type.icon;
        }

    }
}












