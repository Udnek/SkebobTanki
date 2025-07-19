using Attribute;
using DefaultNamespace;
using UI;
using UnityEngine;

namespace Tank
{
    public class Tank : MonoBehaviour
    {
        protected Attributes attributes;
        public double health;
        private void Awake()
        {
            attributes = gameObject.GetOrAddComponent<Attributes>();
            AddHitReceiverToCollidableChildren(transform);
        }

        private void Start()
        {
            attributes = gameObject.GetOrAddComponent<Attributes>();
            attributes.SetBaseValue(AttributeType.HEALTH, 10);
            health = attributes.GetValue(AttributeType.HEALTH);
        }

        protected void AddHitReceiverToCollidableChildren(Transform parent)
        {
            foreach (Transform childTransform in parent)
            {
                AddHitReceiverToCollidableChildren(childTransform);
                if (!ShouldAddHitReceiver(childTransform)) continue;
                childTransform.GetOrAddComponent<HitReceiver>().Listeners += 
                    o => Debug.Log("HIT");
            }
        }

        protected bool ShouldAddHitReceiver(Transform child) => child.GetComponent<Collider>() != null;

        private void Update()
        {
            UIManager.instance.attributeText.text = $"{health}/{attributes.GetValue(AttributeType.HEALTH)}";
        }
    }
}












