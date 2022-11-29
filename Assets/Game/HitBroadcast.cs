using System.Linq;
using UnityEngine;

namespace DropOfAHat.Game {
    public class HitBroadcast : MonoBehaviour {
        [SerializeField]
        private string[] _tagsFilter;
        [SerializeField]
        private bool _processTriggers = false;
        [SerializeField]
        private bool _processCollisions = false;
        [SerializeField]
        private bool _processEnters = false;
        [SerializeField]
        private bool _processExits = false;

        private GameEvents _events;

        private void Start() {
            _events = FindObjectOfType<GameEvents>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (_processTriggers && _processEnters) {
                ProcessHit(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (_processTriggers && _processExits) {
                ProcessLeft(other.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (_processCollisions && _processEnters) {
                ProcessHit(other.gameObject);
            }
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (_processCollisions && _processExits) {
                ProcessLeft(other.gameObject);
            }
        }

        private void ProcessLeft(GameObject collidedWith) {
            if (_tagsFilter.Contains(collidedWith.tag)) {
                _events.Send<LeftEvent>(new LeftEvent(gameObject, collidedWith));
            }
        }

        private void ProcessHit(GameObject collidedWith) {
            if (_tagsFilter.Contains(collidedWith.tag)) {
                _events.Send<HitEvent>(new HitEvent(gameObject, collidedWith));
            }
        }

        public struct HitEvent {
            public GameObject Sender { get; }
            public GameObject Collider { get; }

            public HitEvent(GameObject sender, GameObject collider) {
                Sender = sender;
                Collider = collider;
            }
        }

        public struct LeftEvent {
            public GameObject Sender { get; }
            public GameObject Collider { get; }

            public LeftEvent(GameObject sender, GameObject collider) {
                Sender = sender;
                Collider = collider;
            }
        }
    }
}
