using Attribute;
using Inventory;
using Item;
using UI;
using UI.Renderer;
using UnityEngine;

public class TestTank : MonoBehaviour
{
    
    private bool invPressed;
    private void Update()
    {
        if (!invPressed && Input.GetKeyDown(KeyCode.E))
        {
            invPressed = true;
            InventoryManager.instance.Toggle(
                new ShopInventoryRenderer(new ShopInventory()), 
                new TankInventoryRenderer(GetComponent<Tank.Tank>().components.Get<PlayerInventory>()));
        }
        else invPressed = false;
    }
}
