using Inventory;

namespace UI.Renderer
{
    public class ShopInventoryRenderer : AbstractInventoryRenderer<ShopInventory>, InventoryListener
    {
        
        protected override ShopInventory inventory { get; set; }
        
        public ShopInventoryRenderer(ShopInventory inventory)
        {
            this.inventory = inventory;
        }
        
        public override void Open()
        {
            base.Open();
            for (var index = 0; index < inventory.slots.Length; index++)
            {
                var slot = inventory.slots[index];
                slots.Add(slot, InventoryManager.instance.CreateSlot(slot, index, 0, InventoryManager.instance.shopSlotsLayer)); 
            }
        }

        public override void OnSlotAdded(Inventory.Slot slot, Row row) { }
    }
}
