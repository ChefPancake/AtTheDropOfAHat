using DropOfAHat.Game;
using DropOfAHat.Player;
using UnityEngine;

namespace DropOfAHat.Hat {
    public class Hat : MonoBehaviour {
        private const string IS_ON_PLAYER_ANIMATION_STATE = "IsOnPlayer";
        private const string VELOCITY_ANIMATION_STATE = "Velocity";

        [SerializeField]
        private GameObject _player;
        [SerializeField]
        private Vector3 _offsetOnPlayer;
        [SerializeField]
        private bool _isOnPlayer = true;

        private bool _inAir = false;
        private Rigidbody2D _rigidBody;
        private Collider2D _collider;
        private GameEvents _events;
        private Animator _animator;

        private void Start() {
            _events = FindObjectOfType<GameEvents>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _animator = GetComponentInChildren<Animator>();
            if (_isOnPlayer) {
                Catch();
            }
            _events.Subscribe<PlayerThrow.HatThrown>(OnThrown);
            _events.Subscribe<LevelEnd.HitEvent>(OnLevelEndHit);
        }

        private void Update() {
            if (_isOnPlayer) {
                transform.position = _player.transform.position + _offsetOnPlayer;
            } else {
                _animator.SetFloat(
                    VELOCITY_ANIMATION_STATE, 
                    _rigidBody.velocity.magnitude);
            }
        }

        public void Catch() {
            _inAir = false;
            _isOnPlayer = true;
            _animator.SetBool(IS_ON_PLAYER_ANIMATION_STATE, true);
            _animator.SetFloat(VELOCITY_ANIMATION_STATE, 0f);
            SetPhysics(false);
            _events.Send(new CaughtEvent());
        }

        private void OnThrown(PlayerThrow.HatThrown thrown) {
            if (_isOnPlayer) {
                _isOnPlayer = false;
                _animator.SetBool(IS_ON_PLAYER_ANIMATION_STATE, false);
                SetPhysics(true);
                _rigidBody.velocity = thrown.ThrowVec;
            }
        }

        private void OnLevelEndHit(LevelEnd.HitEvent _) {
            if (!_isOnPlayer) {
                Catch();
            }
        }

        private void SetPhysics(bool enabled) {
            _collider.enabled = enabled;
            _rigidBody.simulated = enabled;
            if (enabled) {
                _rigidBody.position = transform.position;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other) {
            if (!_isOnPlayer && other.gameObject.CompareTag("Player")) {
                _inAir = true;
            }
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (_inAir) {
                if (other.gameObject.CompareTag("Player")) {
                    Catch();
                }
                if (other.gameObject.CompareTag("World")) {
                    _events.Send(new DroppedEvent());
                }
            }
        }

        public struct DroppedEvent { }
        public struct CaughtEvent { }
    }
}