using System;
using Item.Components;
using Tank.Parts;
using UnityEngine;

namespace Tank
{
    public class Hull : BasePart
    {
        [field: SerializeField]
        public Transform tracksPosition { get; private set; }
        [field: SerializeField]
        public Transform turretPosition { get; private set; }

        protected override void AddSlots(SlotConsumer consumer)
        {
            consumer(SlotType.BOTTOM, tracksPosition);
            consumer(SlotType.TOP, turretPosition);
        }
    }
}