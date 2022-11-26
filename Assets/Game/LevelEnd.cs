using System.Linq;
using UnityEngine;

namespace DropOfAHat.Game {
    public class LevelEnd : MonoBehaviour {
        [SerializeField]
        private string[] _tagFilter;

        private GameEvents _events;

        private void Start() {
            _events = FindObjectOfType<GameEvents>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (_tagFilter.Contains(other.gameObject.tag)) {
                _events.Send(new HitEvent(other.gameObject.tag));
            }
        }

        public struct HitEvent {
            public string Tag { get; }

            public HitEvent(string tag) =>
                Tag = tag;
        }
    }
}
