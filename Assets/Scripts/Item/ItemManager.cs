using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Item
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance { get; private set; }

        [field: SerializeField] public ConstructableItemType TURRET { get; private set; }
        [field: SerializeField] public ConstructableItemType GUN_BARREL { get; private set; }
        [field: SerializeField] public ConstructableItemType HULL { get; private set; }
        [field: SerializeField] public ConstructableItemType TRACKS { get; private set; }
        [field: SerializeField] public ConstructableItemType DRUM { get; private set; }
        [field: SerializeField] public ConstructableItemType RADAR { get; private set; }

        public List<ItemType> all
        {
            get
            {
                List<ItemType> items = new();
                var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    if (field.FieldType.IsAssignableFrom(typeof(ItemType))) continue;
                    items.Add((ItemType)field.GetValue(this));
                }

                return items;
            }
        }

        private ItemManager() => instance = this;

        private void Awake()
        {
            foreach (var type in all) type.Awake();
        }
    }
}
