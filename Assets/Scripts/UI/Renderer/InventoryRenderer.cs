using JetBrains.Annotations;

namespace UI.Renderer
{
    public interface InventoryRenderer
    {
        
        [CanBeNull] Slot GetSlot(Inventory.Slot slot);
        void Close();
        void Open();
    }
}