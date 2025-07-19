using System.Collections.Generic;
using UnityEngine;
using Math = System.Math;

namespace Attribute
{
    public class Attributes : MonoBehaviour
    {
        private readonly Dictionary<AttributeType, List<AttributeModifier>> modifiers = new();
        private readonly Dictionary<AttributeType, double> cachedValues = new();
        private readonly Dictionary<AttributeType, double> baseValues = new();

        public double GetValue(AttributeType attribute) => cachedValues.GetValueOrDefault(attribute, GetBaseValue(attribute));
        public double GetBaseValue(AttributeType attribute) => baseValues.GetValueOrDefault(attribute, attribute.defaultValue);
        public void SetBaseValue(AttributeType attribute, double value) => baseValues[attribute] = value;
        
        public void AddModifier(AttributeType attribute, AttributeModifier modifier, bool doRecalculate = true)
        {
            var list = modifiers.GetValueOrDefault(attribute, null);
            if (list == null) {
                list = new List<AttributeModifier> { modifier };
                modifiers.Add(attribute, list);
            }
            else list.Add(modifier);
            if (doRecalculate) Recalculate(attribute);
        }

        public void RemoveModifier(AttributeType attribute, AttributeModifier modifier, bool doRecalculate = true)
        {
            modifiers.TryGetValue(attribute, out var list);
            list?.Remove(modifier);
            if (doRecalculate) Recalculate(attribute);
        }
        
        
        public void AddAllModifiers(Dictionary<AttributeType, List<AttributeModifier>> modifiers)
        {
            foreach (var pair in modifiers)
            {
                pair.Value.ForEach(modifier => AddModifier(pair.Key, modifier, false));
                Recalculate(pair.Key);
            }
        }

        public void RemoveAllModifiers(Dictionary<AttributeType, List<AttributeModifier>> modifiers)
        {
            foreach (var pair in modifiers)
            {
                pair.Value.ForEach(modifier => RemoveModifier(pair.Key, modifier, false));
                Recalculate(pair.Key);
            }
        }
        
        private void Recalculate(AttributeType attribute)
        {
            var value = GetBaseValue(attribute);
            var muls = 1.0;
            foreach (var modifier in modifiers.GetValueOrDefault(attribute, new List<AttributeModifier>()))
            {
                switch (modifier.operation)
                {
                    case AttributeModifier.Operation.ADD: value += modifier.value; break;
                    case AttributeModifier.Operation.MULTIPLY: muls *= modifier.value; break;
                }
            }

            value *= muls;
            cachedValues[attribute] = Math.Clamp(value, attribute.minValue, attribute.maxValue);
        }
    }
}