using ECS;
using Tank.Parts;

namespace Item
{
    public class PartPrefab : CustomComponent<ItemType>
    {
        public readonly Part prefab;
        public PartPrefab(Part prefab) => this.prefab = prefab;
    }
}