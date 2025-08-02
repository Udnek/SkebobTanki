namespace Item.Components
{
    public class SlotType
    {
        
        public static readonly SlotType SHOP = new("shop");
        public static readonly SlotType HULL = new("hull");
        public static readonly SlotType BACKPACK = new("backpack");
        // generic
        public static readonly SlotType BOTTOM = new("bottom");
        public static readonly SlotType TOP = new("top");
        public static readonly SlotType LEFT = new("left");
        public static readonly SlotType RIGHT = new("right");
        public static readonly SlotType FRONT = new("front");
        public static readonly SlotType BACK = new("back");
        
        public readonly string name;
        public SlotType(string name) => this.name = name;

        public override string ToString() => $"Slot '{name}'";
    }
}