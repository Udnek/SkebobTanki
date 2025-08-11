using JetBrains.Annotations;
using UnityEngine;

namespace Inventory.SlotTypes
{
    public interface SlotType
    {
        string name { get; }
        [CanBeNull] Sprite icon { get; }
        bool isHull => (ConstructableSlotType)this == SlotManager.instance.HULL;
    }
}