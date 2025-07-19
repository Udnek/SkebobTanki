using System.Collections.Generic;

namespace Attribute
{
    public class AttributeType
    {
        public static readonly List<AttributeType> ALL = new();
        
        public static readonly AttributeType SPEED = Register(new AttributeType("speed", 0.0, 0.0, 1024.0));
        public static readonly AttributeType HEALTH = Register(new AttributeType("health",1, 1, 1024.0));
        
        public readonly string name;
        public readonly double defaultValue;
        public readonly double minValue;
        public readonly double maxValue;

        private AttributeType(string name, double defaultValue, double minValue, double maxValue){
            this.name = name;
            this.defaultValue = defaultValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
        
        private static AttributeType Register(AttributeType attribute) 
        {
            ALL.Add(attribute);
            return attribute;
        }
    }
}
