using ECS;
using ECS.Attribute;
using UnityEngine;

public class Tank : Entity
{
    public Transform[] upgradePoses;
    public GameObject[] upgrades;
    
    void Start()
    {
        components.Add(new AttributeComponent());
        components.Get<AttributeComponent>()!.AddModifier(
            Attribute.SPEED, new AttributeModifier(AttributeModifier.Operation.Add, 3.0));

        
        for (var i = 0; i < upgradePoses.Length; i++)
        { 
            upgrades[i].transform.position = upgradePoses[i].position;
            upgrades[i].transform.rotation = upgradePoses[i].rotation;
        }
    }
}
