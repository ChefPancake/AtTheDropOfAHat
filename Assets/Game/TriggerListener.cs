using System;
using UnityEngine;

namespace DropOfAHat.Game {
    public class TriggerListener : MonoBehaviour {
        
        public event Action Triggered;

        private void OnTriggerEnter2D(Collider2D other) =>
            Triggered?.Invoke();
    }
}