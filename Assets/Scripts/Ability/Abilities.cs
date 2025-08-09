using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ability
{
    public class Abilities : MonoBehaviour
    {
        private int amountField = 1;

        public event Action<int> abilitiesChanged;
        
        public int amount
        {
            get => amountField;
            set
            {
                abilitiesChanged?.Invoke(value);
                amountField = value;
            }
        }
    }
}