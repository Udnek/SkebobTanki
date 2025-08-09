using Inventory;
using JetBrains.Annotations;
using UI.Slots;

namespace UI.InventoryAgents
{
    public interface InventoryAgent
    {
        [CanBeNull] InventorySlot GetSlot(Slot slot);
        void Close();
        void Open();
    }
}