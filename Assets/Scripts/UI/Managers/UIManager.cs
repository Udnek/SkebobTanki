using TMPro;
using UnityEngine;

namespace UI.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance { get; private set; }
        [Header("Text")]
        [field: SerializeField] public TextMeshProUGUI attributeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI speedText { get; private set; }
        private void Awake() => instance = this;
    }
}












