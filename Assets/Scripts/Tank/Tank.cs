using System.Collections.Generic;
using Attribute;
using ECS;
using Inventory;
using Item;
using Item.Components;
using Tank.Parts;
using UI;
using UnityEngine;
using Slot = Inventory.Slot;

namespace Tank
{
    public class Tank : MonoBehaviour, Entity<Tank>
    {
        public ComponentMap<Tank> components { get; } = new();
        
        protected double health;
        protected Attributes attributes;
        protected PlayerInventory inventory;
        private void Awake()
        {
            inventory = new PlayerInventory();
            inventory.onItemSet += (slot, oldItem) => OnInventoryChanged(slot, oldItem, true);
            inventory.onItemSwapped += (slot, withSlot) => OnInventoryChanged(slot, withSlot.item, withSlot.inventory != inventory);
            components.Add(inventory);
            
            attributes = gameObject.GetOrAddComponent<Attributes>();
        }
        
        private void Start()
        {
            attributes = gameObject.GetOrAddComponent<Attributes>();
            attributes.SetBaseValue(AttributeType.HEALTH, 10);
            health = attributes.GetValue(AttributeType.HEALTH);
            inventory.hullSlot.Set(new ItemStack(ItemManager.instance.HULL));
        }

        protected void AddHitReceiverToCollidableObject(Transform parent)
        {
            if (ShouldAddHitReceiver(parent)) parent.GetOrAddComponent<HitReceiver>().Listeners += OnHit;
            foreach (Transform childTransform in parent)
            {
                AddHitReceiverToCollidableObject(childTransform);
                if (!ShouldAddHitReceiver(childTransform)) continue;
                childTransform.GetOrAddComponent<HitReceiver>().Listeners += OnHit;
            }
        }

        private void OnInventoryChanged(Slot slot, ItemStack oldItem, bool removeOld)
        {
            if (removeOld) oldItem?.components?.Get<InitiatedPart>()?.Destroy();
            if (slot.item is null) return;
            slot.item.components.Get<InitiatedPart>()?.Destroy();
            var newPhysicalPart = slot.item.type.components.Get<PartPrefab>();
            if (newPhysicalPart == null) return;
            if (slot.type == SlotType.HULL)
            {
                var initiatedPart = slot.item.components.GetOrAdd(() => new InitiatedPart());
                var part = Instantiate(newPhysicalPart.prefab, transform).GetComponent<Part>();
                initiatedPart.script = part;
                OnPartSet(part);
            }
            else {
                var providerPart = slot.parent?.item?.components?.Get<InitiatedPart>()?.script;
                if (providerPart == null) return;
                var position = providerPart.slots.GetValueOrDefault(slot.type, null);
                if (position is null) return;
                var part = Instantiate(newPhysicalPart.prefab, position).GetComponent<Part>();
                slot.item.components.GetOrAdd(() => new InitiatedPart()).script = part;
                part.ApplyRotation();
                OnPartSet(part);
            }
        }

        protected void OnPartSet(Part part)
        {
            AddHitReceiverToCollidableObject(part.transform);
            part.tank = this;
        }

        
        protected void OnHit(HitEvent hitEvent)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(hitEvent.direction * hitEvent.force, hitEvent.hit.point);
        }

        protected bool ShouldAddHitReceiver(Transform obj) => obj.GetComponent<Collider>() != null;

        private void Update()
        {
            UIManager.instance.attributeText.text = $"{inventory.storageRows.Count}";
        }

        
    }
}












