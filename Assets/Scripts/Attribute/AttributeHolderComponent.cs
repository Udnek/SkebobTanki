using System.Collections.Generic;
using ECS;
using Item;

namespace Attribute
{
    public class AttributeHolderComponent : CustomComponent<ItemType>
    {
        public Dictionary<AttributeType, List<AttributeModifier>> modifiers { get; } = new(); 
        
        public void AddModifier(AttributeType attribute, AttributeModifier modifier)
        {
            var list = modifiers.GetValueOrDefault(attribute, null);
            if (list == null) {
                list = new List<AttributeModifier> { modifier };
                modifiers.Add(attribute, list);
            }
            else list.Add(modifier);
        }
    }
}
