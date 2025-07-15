using System;
using System.Collections.Generic;
using System.Text;
using ECS;
using ECS.Attribute;
using UI;
using UnityEngine;
using Attribute = ECS.Attribute.Attribute;

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

        
        for (var i = 0; i < upgradePoses.Length; i++)
        { 
            upgrades[i].transform.position = upgradePoses[i].position;
            upgrades[i].transform.rotation = upgradePoses[i].rotation;
        }
    }

    private void Update()
    {
        var builder = new StringBuilder();
        foreach (var attribute in Attribute.ALL)
        {
            builder.AppendLine($"{attribute.name}: {attributes.Get(attribute)}");
        }
        UIManager.Instance.attributeText.text = builder.ToString();
        UIManager.Instance.speedText.text = "TODO";
    }
}
