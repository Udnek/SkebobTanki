using System;
using Ability;
using TMPro;
using UI.Slots;
using UnityEngine;

namespace UI.Managers
{
    public class HotbarManager : MonoBehaviour, SlotListener
    { 
        public static HotbarManager instance { get; private set; }
        [field: SerializeField] public RectTransform hotbar { get; private set; }
        [field: SerializeField] public RectTransform healthBarSize { get; private set; }
        [field: SerializeField] public RectTransform healthBar { get; private set; }
        [field: SerializeField] public Transform abilitySlotsLayer { get; private set; }
        [field: SerializeField] public TextMeshProUGUI healthText { get; private set; }
        [field: SerializeField] public AbilitySlot slotPrefab { get; private set; }
        private void Awake() => instance = this;

        public void UpdateHealth(float current, float max)
        {
            var size = healthBarSize.rect.size;
            size.x *= current / max;
            healthBar.sizeDelta = size;
            healthText.text = $"{current:F1}/{max:F1}";
        }
        
        public void UpdateAbilities(Abilities abilities)
        {
            int amount = Math.Max(1, abilities.amount);
            hotbar.sizeDelta += new Vector2((amount - 1)*slotPrefab.size, 0);
            for (int i = 0; i < amount; i++) 
                slotPrefab.InstantiateNew(this, abilitySlotsLayer, i, 0);
        }

        public void OnSlotClicked(AbstractSlot abstractSlot) { }

        public void OnSlotHovered(AbstractSlot abstractSlot) { }

        public void OnSlotUnhovered(AbstractSlot abstractSlot) { }
    }
}


