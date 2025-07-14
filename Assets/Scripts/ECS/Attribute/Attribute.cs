namespace ECS.Attribute
{
    public class Attribute
    {
        public static readonly Attribute SPEED = new Attribute(0.0, 0.0, 1024.0);
        public static readonly Attribute HEALTH = new Attribute(1, 1, 1024.0);
    
        public readonly double defaultValue;
        public readonly double minValue;
        public readonly double maxValue;

        private Attribute(double defaultValue, double minValue, double maxValue){
            this.defaultValue = defaultValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }
}
