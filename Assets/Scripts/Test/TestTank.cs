using Attribute;
using Inventory;
using Item;
using UI;
using UI.Renderer;
using UnityEngine;

public class TestTank : MonoBehaviour
{
    
    private void Start()
    {
        var inventory = gameObject.GetOrAddComponent<PlayerInventory>();
        inventory.hullSlots.main.SetNoLeftOver(new ItemStack(ItemManager.instance.HULL));
        inventory.turretSlots.main.SetNoLeftOver(new ItemStack(ItemManager.instance.TURRET));
        
    }
    private bool invPressed;
    private void Update()
    {
        if (!invPressed && Input.GetKeyDown(KeyCode.E))
        {
            invPressed = true;
            InventoryManager.instance.Toggle(new ShopInventoryRenderer(new ShopInventory()), new TankInventoryRenderer(GetComponent<Inventory.PlayerInventory>()));
        }
        else invPressed = false;
    }
}
