using Attribute;
using UI;
using UnityEngine;

namespace Tank
{
    public class Tank : MonoBehaviour
    {
        // PARTS
        public Hull hull; 
        public Barrel barrel;
        public Tracks tracks;
        public Turret turret;
        //
        protected double health;
        protected Attributes attributes;
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
            InitializeParts();
        }

        protected void InitializeParts()
        {
            hull = Instantiate(hull, transform);
            tracks = Instantiate(tracks, hull.tracksPosition);
            turret = Instantiate(turret, hull.turretPosition);
            barrel = Instantiate(barrel, turret.barrelPosition);
        }

        protected void AddHitReceiverToCollidableChildren(Transform parent)
        {
            foreach (Transform childTransform in parent)
            {
                AddHitReceiverToCollidableChildren(childTransform);
                if (!ShouldAddHitReceiver(childTransform)) continue;
                childTransform.GetOrAddComponent<HitReceiver>().Listeners += OnHit;
            }
        }
        protected void OnHit(HitEvent hitEvent)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(hitEvent.direction * hitEvent.force, hitEvent.hit.point);
        }

        protected bool ShouldAddHitReceiver(Transform child) => child.GetComponent<Collider>() != null;

        private void Update()
        {
            // UIManager.instance.attributeText.text = $"{health}/{attributes.GetValue(AttributeType.HEALTH)}";
        }
    }
}












