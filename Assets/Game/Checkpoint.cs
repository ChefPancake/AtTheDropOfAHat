using UnityEngine;

namespace DropOfAHat.Game {
    public class Checkpoint : MonoBehaviour {
        [SerializeField]
        private uint _ordinal;

        public uint Ordinal => _ordinal;
        
        private GameEvents _events;

        private void Start() {
            _events = FindObjectOfType<GameEvents>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                _events.Send<HitEvent>(new HitEvent(_ordinal));
            }
        }

        public struct HitEvent {
            public uint Ordinal { get; }

            public HitEvent(uint ordinal) => 
                Ordinal = ordinal;
        }
    }
}
