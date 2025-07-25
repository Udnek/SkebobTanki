using System;
using System.Collections.Generic;
using System.Linq;
using Item.Components;
using JetBrains.Annotations;
using UnityEngine;

namespace Tank.Parts
{
    public abstract class Part : MonoBehaviour
    {
        public delegate void SlotConsumer(SlotType slot, Transform position);
        public Dictionary<SlotType, Transform> slots { get; } = new();

        protected abstract void AddSlots(SlotConsumer consumer);
        public abstract void ApplyRotation();

        private void OnValidate() => AddSlots((slot, position) => slots[slot] = position);
        private void Awake() => AddSlots((slot, position) => slots[slot] = position);
    }
}