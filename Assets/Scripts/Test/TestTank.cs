using Inventory;
using UI.InventoryAgents;
using UI.Managers;
using UI.Managers.Managers;
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
            controls.Player.Inventory.performed += (context) => LayoutMenuManager.instance.Toggle(
                new TankInventoryAgent(GetComponent<Tank.Tank>().components.Get<PlayerInventory>())
            );
        }
    }
}
