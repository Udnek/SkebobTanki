namespace ECS.Attribute
{
    public class AttributeModifier
    { 
        public double value { get; private set; }
        public Operation operation {get; private set;}

        public AttributeModifier(Operation operation, double value)
        {
            this.operation = operation;
            this.value = value;
        }
    
        public enum Operation
        {
            ADD,
            MULTIPLY
        }
    }
}
