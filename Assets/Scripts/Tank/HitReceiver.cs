using System;
using UnityEngine;

namespace Tank
{
    public class HitReceiver: MonoBehaviour
    {
        public event Action<RaycastHit> Listeners;

        public void Hit(RaycastHit hit)
        {
            Listeners?.Invoke(hit);
        }
    }
}