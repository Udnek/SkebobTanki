using UI.InventoryAgents;
using UnityEngine;

namespace UI.Managers.Managers
{
    public class LayoutMenuManager : MonoBehaviour
    {
        public static LayoutMenuManager instance { get; private set; }
        [field: SerializeField] public GameObject layer { get; private set; }
        public bool isOpened { get; private set; } = true;
        
        private void Awake() => instance = this;
        private void Start()
        {
            isOpened = true;
            Close();
        }

        private void Open(params InventoryAgent[] renderers)
        {
            if (isOpened) return;
            isOpened = true;
            InventoryManager.instance.Open(renderers);
            layer.SetActive(true);
        }

        private void Close()
        {
            if (!isOpened) return;
            isOpened = false;
            InventoryManager.instance.Close();
            TemporalManager.instance.Clear();
            layer.SetActive(false);
        }

        public void Toggle(params InventoryAgent[] renderers)
        {
            if (isOpened) Close();
            else Open(renderers);
        }
    }
}