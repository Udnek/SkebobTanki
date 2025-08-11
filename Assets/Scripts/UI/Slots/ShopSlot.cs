using Item;
using UI.Managers;

namespace UI.Slots
{
    public class ShopSlot : AbstractSlot
    {
        public override Type type => Type.SHOP;
        
        private ItemType itemField;
        public ItemType item
        {
            get => itemField;
            set
            {
                itemField = value;
                gameObject.name = item.name;
                var ic = Instantiate(InventoryManager.instance.itemStackPrefab, transform, false);
                icon = ic;
                ic.image.sprite = item.icon;
            }
        }

        public override bool isEmpty => false;

        public override void UpdateIcon() { }

    }
}












