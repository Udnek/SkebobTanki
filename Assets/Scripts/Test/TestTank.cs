using Attribute;
using DefaultNamespace;
using Item;
using UI;
using UnityEngine;

public class TestTank : MonoBehaviour
{
    public Transform[] upgradePoses;
    public GameObject[] upgrades;

    private Attributes attributes;

    private void Start()
    {
        var inventory = gameObject.GetOrAddComponent<Inventory>();
        inventory.SetItem(0, new ItemStack(ConstructableItemType.ALL[0]));
        inventory.SetItem(1, new ItemStack(ConstructableItemType.ALL[1]));
        
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
            InventoryManager.instance.Toggle(GetComponent<Inventory>());
        }
        else invPressed = false;
    }
}
