using JetBrains.Annotations;
using UnityEngine;

namespace Item
{
    public class Inventory : MonoBehaviour
    {
        public const int SIZE = 3;

        private ItemStack[] items = new ItemStack[SIZE];
        
        public void Clear() => items = new ItemStack[SIZE];
        public void SetItem(int index, ItemStack itemStack) => items[index] = itemStack;
        
        [CanBeNull] public ItemStack GetItem(int index) => items[index];
        public ItemStack[] GetItems() => items;
    }
}
