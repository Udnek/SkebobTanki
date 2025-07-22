namespace Item
{
    public class ItemStack
    {
        
        public readonly ItemType type;
        public ItemStack(ItemType type) => this.type = type;
        
        public ItemStack Clone() => new(type);
    }
}
