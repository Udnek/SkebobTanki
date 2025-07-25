namespace Item.Components
{
    public class SlotType
    {
        
        public static readonly SlotType SHOP = new SlotType("shop");
        public static readonly SlotType HULL = new SlotType("hull");
        public static readonly SlotType BACKPACK = new SlotType("backpack");
        // generic
        public static readonly SlotType BOTTOM = new SlotType("bottom");
        public static readonly SlotType TOP = new SlotType("top");
        public static readonly SlotType LEFT = new SlotType("left");
        public static readonly SlotType RIGHT = new SlotType("right");
        public static readonly SlotType FRONT = new SlotType("front");
        public static readonly SlotType BACK = new SlotType("back");
        
        public readonly string name;
        public SlotType(string name) => this.name = name;

        public override string ToString() => $"Slot '{name}'";
    }
}