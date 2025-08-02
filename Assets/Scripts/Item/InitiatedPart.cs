using ECS;
using JetBrains.Annotations;
using Tank.Parts;
using UnityEngine;

namespace Item
{
    public class InitiatedPart : CustomComponent<ItemStack>
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