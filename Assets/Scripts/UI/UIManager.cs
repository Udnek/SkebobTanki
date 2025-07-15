using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI attributeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI speedText { get; private set; }
        
        public static UIManager Instance { get; private set; }

        private UIManager() {}
        private void Awake() => Instance = this;
    }
}
