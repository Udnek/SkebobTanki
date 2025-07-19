using TMPro;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance { get; private set; }
        
        [field: SerializeField] public TextMeshProUGUI attributeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI speedText { get; private set; }
        [field: SerializeField] public GameObject tooltipPrefab {get; private set;}  
        private UIManager() {}
        private void Awake() => instance = this;
    }
}












