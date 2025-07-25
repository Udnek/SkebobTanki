using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace Item
{
    public interface ItemType : Entity<ItemType>{
        string name { get; }
        string description { get; }
        Sprite icon { get; }

        public void Start();
    }
}
