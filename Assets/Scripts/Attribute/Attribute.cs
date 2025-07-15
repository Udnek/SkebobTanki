using System.Collections.Generic;

namespace ECS.Attribute
{
    public class Attribute
    {
        public static readonly List<Attribute> ALL = new();
        
        public static readonly Attribute SPEED = Register(new Attribute("speed", 0.0, 0.0, 1024.0));
        public static readonly Attribute HEALTH = Register(new Attribute("health",1, 1, 1024.0));

        
        
        public readonly string name;
        public readonly double defaultValue;
        public readonly double minValue;
        public readonly double maxValue;

        private Attribute(string name, double defaultValue, double minValue, double maxValue){
            this.name = name;
            this.defaultValue = defaultValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
        
        private static Attribute Register(Attribute attribute) 
        {
            ALL.Add(attribute);
            return attribute;
        }
    }
}
