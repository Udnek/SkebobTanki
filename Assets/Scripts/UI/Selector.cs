using UI.Managers;
using UnityEngine;

namespace UI
{
    public class Selector : MonoBehaviour
    {
        public void Destroy() => Destroy(gameObject);
        public Selector InstantiateNew(Transform parent)
        {
            var selector = Instantiate(this, parent);
            InventoryManager.instance.currentSelector = selector;
            return selector;
        }
    }
}