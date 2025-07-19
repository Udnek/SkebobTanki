using ECS;
using UnityEngine;

namespace Item
{
    public interface ItemType : Entity{
        string name { get; }
        string description { get; }
        Texture2D iconTexture { get; }
    }
}
