using System;
using System.Linq;
using UnityEngine;

namespace DropOfAHat.Game {
    public class TriggerListener : MonoBehaviour {
        [SerializeField]
        private string[] _tagFilter;

        public event Action Triggered;

        private void OnTriggerEnter2D(Collider2D other) {
            if (_tagFilter.Any(other.CompareTag)) {
                Triggered?.Invoke();
            }
        }
    }
}