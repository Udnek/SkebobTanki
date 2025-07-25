using ECS;

namespace Item
{
    public class ItemStack : AbstractEntity<ItemStack>
    {
        public readonly ItemType type;
        public ItemStack(ItemType type) => this.type = type;
    }
}
