using ECS;
using JetBrains.Annotations;
using Tank.Parts;
using UnityEngine;

namespace Item
{
    public class CreatedPart : CustomComponent<ItemStack>
    {
        [CanBeNull]
        public Part script { get; set; }

        public void Destroy()
        {
            if (script != null) Object.Destroy(script.gameObject);
            script = null;
        }
    }
}