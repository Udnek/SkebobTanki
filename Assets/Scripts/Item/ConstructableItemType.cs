using System.Linq;
using ECS;
using Tank.Parts;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "Scriptable Objects/ItemType")]
    public class ConstructableItemType : ScriptableObject, ItemType
    {
        public ComponentMap<ItemType> components { get; } = new();
        [field: SerializeField] public Part partPrefab {get; private set; }
        [field: SerializeField] public new string name { get; protected set; }
        [field: SerializeField] public string description { get; protected set; }
        [field: SerializeField] public Texture2D iconTexture { get; protected set; }

        public Sprite icon => 
            Sprite.Create(iconTexture, new Rect(0f, 0f, iconTexture.width, iconTexture.height), Vector2.zero);

        public void Start()
        {
            if (partPrefab is null) return;
            if (partPrefab.slots.Count > 0) 
                components.Add(new ExtraSlots(partPrefab.slots.Keys.ToArray()));
            components.Add(new PhysicalPart(partPrefab));
        }
    }
}
