using System.Collections.Generic;

namespace ECS.Attribute
{
    public class AttributeComponent : CustomComponent
    {
        private Dictionary<Attribute, List<AttributeModifier>> modifiers = new();
        private Dictionary<Attribute, double> cachedValues = new();

        public double Get(Attribute attribute) => cachedValues.GetValueOrDefault(attribute, attribute.defaultValue);

        public Dictionary<Attribute, List<AttributeModifier>> GetAll() => modifiers;

        public void AddModifier(Attribute attribute, AttributeModifier modifier, bool doRecalculate = true)
        {
            var list = modifiers.GetValueOrDefault(attribute, null);
            if (list == null) {
                list = new List<AttributeModifier> { modifier };
                modifiers.Add(attribute, list);
            }
            else list.Add(modifier);
            if (doRecalculate) Recalculate(attribute);
        }

        public void RemoveModifier(Attribute attribute, AttributeModifier modifier, bool doRecalculate = true)
        {
            modifiers.TryGetValue(attribute, out var list);
            list?.Remove(modifier);
            if (doRecalculate) Recalculate(attribute);
        }
        
        
        public void AddAllModifiers(Dictionary<Attribute, List<AttributeModifier>> modifiers)
        {
            foreach (var pair in modifiers)
            {
                pair.Value.ForEach(modifier => AddModifier(pair.Key, modifier, false));
                Recalculate(pair.Key);
            }
        }

        public void RemoveAllModifiers(Dictionary<Attribute, List<AttributeModifier>> modifiers)
        {
            foreach (var pair in modifiers)
            {
                pair.Value.ForEach(modifier => RemoveModifier(pair.Key, modifier, false));
                Recalculate(pair.Key);
            }
        }
        
        private void Recalculate(Attribute attribute)
        {
            var value = attribute.defaultValue;
            var muls = 1.0;
            foreach (var modifier in modifiers.GetValueOrDefault(attribute, new List<AttributeModifier>()))
            {
                switch (modifier.operation)
                {
                    case AttributeModifier.Operation.Add: value += modifier.value; break;
                    case AttributeModifier.Operation.Multiply: muls *= modifier.value; break;
                }
            }

            value *= muls;
            cachedValues[attribute] = value;
        }
    }
}
