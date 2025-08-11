using System.Collections.Generic;
using System.Reflection;
using Item;
using UnityEngine;

namespace Inventory.SlotTypes
{
    public class SlotManager : MonoBehaviour
    { 
        public static SlotManager instance { get; private set; }

        [field: SerializeField] public ConstructableSlotType HULL {get; private set;}
        [field: SerializeField] public ConstructableSlotType BACKPACK {get; private set;}
        [field: SerializeField] public ConstructableSlotType BOTTOM {get; private set;}
        [field: SerializeField] public ConstructableSlotType TOP {get; private set;}
        [field: SerializeField] public ConstructableSlotType LEFT {get; private set;}
        [field: SerializeField] public ConstructableSlotType RIGHT {get; private set;}
        [field: SerializeField] public ConstructableSlotType FRONT {get; private set;}
        [field: SerializeField] public ConstructableSlotType BACK {get; private set;}
        
        private SlotManager() => instance = this;
    }
}