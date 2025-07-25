namespace Item.Components
{
    public class SlotType
    {
        
        public static readonly SlotType BOTTOM = new SlotType("bottom");
        public static readonly SlotType TOP = new SlotType("top");
        public static readonly SlotType LEFT = new SlotType("left");
        public static readonly SlotType RIGHT = new SlotType("right");
        public static readonly SlotType FRONT = new SlotType("front");
        public static readonly SlotType BACK = new SlotType("back");
        
        private string name;
        public SlotType(string name) => this.name = name;
    }
}