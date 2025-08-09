using System;
using Inventory;
using Item;
using UI.Slots;
using UnityEngine;

namespace UI.Managers
{
    public class ShopManager : MonoBehaviour, SlotListener
    {
        private const int ITEMS_PER_ROW = 4;
        public static ShopManager instance { get; private set; }
        [field: SerializeField] public ShopSlot slotPrefab {get; private set;}
        [field: SerializeField] public Transform layer {get; private set;}
        private void Awake() => instance = this;

        private void Start()
        {
            int column = 0;
            int row = 0;
            foreach (var item in ItemManager.instance.all)
            {
                if (column == ITEMS_PER_ROW)
                {
                    row -= 1;
                    column = 0;
                }
                var slot = Instantiate(slotPrefab, layer);
                var rectTransform = slot.GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector3(column*slot.size, row*slot.size, 0);
                slot.listener = this;
                slot.item = item;
                column++;
            }
        }

        public void OnSlotClicked(AbstractSlot abstractSlot)
        {
            var backpack = PlayerManager.instance.player.components.Get<PlayerInventory>()
                .AddToBackpack(new ItemStack(abstractSlot.AsShopSlot()!.item));
            if (backpack is null) return;
            TemporalManager.instance.SwapThisSlotAnimation(InventoryManager.instance.GetSlot(backpack), abstractSlot);
        }

        public void OnSlotHovered(AbstractSlot abstractSlot)
        {
            var manager = TemporalManager.instance;
            var slot = abstractSlot.AsShopSlot()!;
            if (slot.isEmpty) return;
            var tooltip = manager.CreateEmptyTooltip();
            var item = slot.item;
            if (string.IsNullOrEmpty(item.description)) tooltip.SetText(item.name);
            else tooltip.SetText(item.name + "\n\n" + item.description);
        }
        public void OnSlotUnhovered(AbstractSlot slot) => TemporalManager.instance.tooltip = null;
    }
}