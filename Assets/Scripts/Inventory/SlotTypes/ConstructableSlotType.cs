using System;
using Item;
using JetBrains.Annotations;
using UnityEngine;

namespace Inventory.SlotTypes
{
    [CreateAssetMenu(fileName = "SlotType", menuName = "Scriptable Objects/SlotType")]
    public class ConstructableSlotType : ScriptableObject, SlotType
    {
        [field: SerializeField] public new string name { get; protected set; }
        [field: SerializeField] public Sprite icon { get; protected set; }

        public override string ToString() => $"Slot '{name}'";
    }
}
