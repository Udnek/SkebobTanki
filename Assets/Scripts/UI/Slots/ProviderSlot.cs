using Inventory;
using UI.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Slots
{
    public class ProviderSlot : InventorySlot
    {
        public override Type type => Type.PROVIDER;
        public override Slot slot
        {
            get => slotField;
            set
            {
                slotField = value;
                gameObject.name = slot.type.name;
                UpdateIcon();
            }
        }
        public override bool isEmpty => false;

        public override void UpdateIcon()
        {
            Clear();
            var ic = Instantiate(InventoryManager.instance.itemStackPrefab, transform, false);
            icon = ic;
            ic.image.sprite = slot.item!.type.icon;  
        }
    }
}












