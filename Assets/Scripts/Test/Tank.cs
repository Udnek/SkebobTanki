using System;
using System.Collections.Generic;
using System.Text;
using ECS;
using ECS.Attribute;
using Item;
using UI;
using UnityEngine;
using Attribute = ECS.Attribute.Attribute;
using Component = System.ComponentModel.Component;

public class Tank : Entity
{
    public Transform[] upgradePoses;
    public GameObject[] upgrades;

    private AttributeComponent attributes;

    private void Start()
    {
        attributes = new AttributeComponent();
        components.Add(attributes);
        attributes.AddModifier(Attribute.SPEED, new AttributeModifier(AttributeModifier.Operation.ADD, 1));
        attributes.AddModifier(Attribute.SPEED, new AttributeModifier(AttributeModifier.Operation.MULTIPLY, 1.2));

        var inventory = new Inventory();
        components.Add(inventory);
        inventory.SetItem(0, new ItemStack(ItemType.ALL[0]));
        inventory.SetItem(1, new ItemStack(ItemType.ALL[1]));
        
        for (var i = 0; i < upgradePoses.Length; i++)
        { 
            upgrades[i].transform.position = upgradePoses[i].position;
            upgrades[i].transform.rotation = upgradePoses[i].rotation;
        }
    }
    private bool invPressed;
    private void Update()
    {
        if (!invPressed && Input.GetKeyDown(KeyCode.E))
        {
            invPressed = true;
            InventoryManager.Instance.Toggle(components.Get<Inventory>());
        }
        else invPressed = false;
        
        var builder = new StringBuilder();
        foreach (var attribute in Attribute.ALL)
        {
            builder.AppendLine($"{attribute.name}: {attributes.Get(attribute)}");
        }
        UIManager.Instance.attributeText.text = builder.ToString();
        UIManager.Instance.speedText.text = "TODO";
    }
}
