using Inventory;
using UI;
using UI.Renderer;
using UnityEngine;

namespace Test
{
    public class TestTank : MonoBehaviour
    {
        private Controls controls;
        
        private void Awake()
        {
            controls = new Controls();
            controls.Player.Inventory.Enable();
            controls.Player.Inventory.performed += (context) => InventoryManager.instance.Toggle(
                new ShopInventoryRenderer(new ShopInventory()),
                new TankInventoryRenderer(GetComponent<Tank.Tank>().components.Get<PlayerInventory>())
            );
        }
    }
}
