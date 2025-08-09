using UI.Managers;
using UnityEngine;

namespace UI
{
    public class Selector : MonoBehaviour
    {
        public void Destroy() => Destroy(gameObject);
        public Selector InstantiateNew()
        {
            var selector = Instantiate(this, TemporalManager.instance.draggableLayer);
            InventoryManager.instance.currentSelector = selector;
            return selector;
        }
    }
}