using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        
        [field: SerializeField] public TextMeshProUGUI attributeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI speedText { get; private set; }
        private UIManager() {}
        private void Awake() => Instance = this;
    }
}












