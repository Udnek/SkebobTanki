using System;
using Inventory;
using JetBrains.Annotations;
using UI.Slots;
using UnityEngine;

namespace UI.Managers
{
    public class TemporalManager : MonoBehaviour
    {
        public static TemporalManager instance { get; private set; }
        
        [SerializeField] private Transform tooltipLayer;
        [field: SerializeField] public Transform draggableLayer { get; private set; }

        [CanBeNull] private AbstractSlot cursorSlotField;
        [CanBeNull]
        public AbstractSlot cursorSlot
        {
            get => cursorSlotField;
            set
            {
                value?.Pickup();
                cursorSlotField = value;
            }
        }
        [CanBeNull] private Tooltip tooltipField;
        [CanBeNull] public Tooltip tooltip
        {
            get => tooltipField;
            set
            {
                if (tooltipField is not null) Destroy(tooltipField.gameObject);
                tooltipField = value;
            }
        }
        private void Awake() => instance = this;
        public bool IsCursorTypeOrNull(AbstractSlot.Type type) => cursorSlot is null || cursorSlot.type == type;

        public Tooltip CreateEmptyTooltip()
        {
            var tt = Instantiate(UIManager.instance.tooltipPrefab, tooltipLayer, false);
            tooltip = tt;
            return tt;
        }
        
        public void AddToDraggableLayer(GameObject go) => go.transform.SetParent(draggableLayer);
        
        private void Update()
        {
            if (cursorSlot is null) return;
            if (cursorSlot.isEmpty) cursorSlot = null; 
            else cursorSlot.icon!.transform.position = Input.mousePosition;
        }
        
        public void SwapThisSlotAnimation(AbstractSlot thisSlot, AbstractSlot withSlot, [CanBeNull] Action onArrival = null)
        {
            thisSlot.UpdateIcon(); 
            if (thisSlot.icon is null) return;
            var icon = thisSlot.icon!;
            AddToDraggableLayer(icon.gameObject);
            icon.transform.position = cursorSlot == withSlot ? withSlot!.icon!.transform.position : withSlot.transform.position;
            SmoothMover.Run(icon.gameObject, 32, thisSlot.transform.position, onArrival);
        }

        public void Clear()
        {
            cursorSlot = null;
            tooltip = null;
        }
    }
}