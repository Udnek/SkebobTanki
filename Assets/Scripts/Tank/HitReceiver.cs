using System;
using UnityEngine;

namespace Tank
{
    public class HitReceiver: MonoBehaviour
    {
        public event Action<HitEvent> Listeners;

        public void Hit(HitEvent hit) => Listeners?.Invoke(hit);
    }
}