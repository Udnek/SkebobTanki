using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "Scriptable Objects/ItemType")]
    public class ItemType : ScriptableObject
    {
        public static List<ItemType> ALL = new();
    
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public Color Color {get; private set; }

        public ItemType() => ALL.Add(this);
    }
}
