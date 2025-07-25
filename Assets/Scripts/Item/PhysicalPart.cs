using ECS;
using Tank;
using Tank.Parts;

namespace Item
{
    public class PhysicalPart : CustomComponent<ItemType>
    {
        public readonly Part prefab;
        public PhysicalPart(Part prefab) => this.prefab = prefab;
    }
}