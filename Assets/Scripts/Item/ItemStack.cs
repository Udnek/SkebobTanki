namespace Item
{
    public class ItemStack
    {
        
        public readonly ConstructableItemType type;
        public ItemStack(ConstructableItemType type) => this.type = type;
    }
}
