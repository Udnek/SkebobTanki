using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "Scriptable Objects/ItemType")]
    public class ConstructableItemType : ScriptableObject, ItemType
    {
        public static readonly List<ConstructableItemType> ALL = new();
    
        public ComponentMap components { get; } = new();
        
        [field: SerializeField] public new string name { get; protected set; }
        [field: SerializeField] public string description { get; protected set; }
        [field: SerializeField] public Texture2D iconTexture { get; protected set; }

        public Sprite icon => 
            Sprite.Create(iconTexture, new Rect(0f, 0f, iconTexture.width, iconTexture.height), Vector2.zero);

        public ConstructableItemType() => ALL.Add(this);
    }
}
