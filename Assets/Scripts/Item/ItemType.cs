using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace Item
{
    public interface ItemType : Entity{
        
        string name { get; }
        string description { get; }
        Sprite icon { get; }
        int extraSlots { get; }
    }
}
