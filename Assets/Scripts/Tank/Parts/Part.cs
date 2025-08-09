using System.Collections.Generic;
using Item.Components;
using UnityEngine;

namespace Tank.Parts
{
    public abstract class Part : MonoBehaviour
    {
        protected delegate void SlotConsumer(SlotType slot, Transform position);
        
        [field: SerializeField] public GameObject modelToOutline { get; private set; }
        
        public Tank tank { get; set; }

        public Dictionary<SlotType, Transform> slots { get; } = new();
        protected abstract void AddSlots(SlotConsumer consumer);
        public abstract void ApplyRotation();

        private void OnValidate() => AddSlots((slot, position) => slots[slot] = position);
        private void Awake() => AddSlots((slot, position) => slots[slot] = position);
    }
}