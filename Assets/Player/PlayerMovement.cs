using DropOfAHat.Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DropOfAHat.Player {
    public class PlayerMovement : MonoBehaviour {
        private const string IS_RUNNING_ANIMATION_STATE = "IsRunning";
        
        [SerializeField]
        private float _moveSpeed = 15f;
        [SerializeField]
        private float _groundAccel = 10f;
        [SerializeField]
        private float _airAccel = 10f;

        private Vector2 _moveInput;
        private bool _isEnabled = false;
        private bool _isGrounded = false;
        private Rigidbody2D _rigidBody;
        private GameEvents _events;
        private Animator _animator;
        private AudioSource _hurtAudio;

        private void Start() {
            _rigidBody = GetComponent<Rigidbody2D>();
            _events = FindObjectOfType<GameEvents>();
            _animator = GetComponentInChildren<Animator>();
            _hurtAudio = GetComponent<AudioSource>();
            _events.Subscribe<PlayerThrow.HatThrown>(OnHatThrown);
            _events.Subscribe<Hat.Hat.CaughtEvent>(OnHatCaught);
        }

        private void Update() {
            if (_isEnabled) {
                Run();
            } else {
                _animator.SetBool(IS_RUNNING_ANIMATION_STATE, false);
            }
        }

        private void Run() {
            var accel = _isGrounded
                ? _groundAccel
                : _airAccel;
            var forceVec = new Vector2(
                _moveInput.x * accel,
                0f);
            _rigidBody.AddForce(forceVec);
            var movingRight = forceVec.x > Vector2.kEpsilon;
            var movingLeft = forceVec.x < -Vector2.kEpsilon;
            _animator.SetBool(
                IS_RUNNING_ANIMATION_STATE,
                movingLeft || movingRight);
            var scaleX = 
                (movingLeft, movingRight) switch {
                    (false, false) => transform.localScale.x,
                    (true, false) => -1f,
                    (false, true) => 1f,
                    (true, true) => 1f, // how tho
                };
            transform.localScale = new Vector2(scaleX, 1f);
        }

        private void LateUpdate() {
            var horiVelMag = Mathf.Abs(_rigidBody.velocity.x);
            if (horiVelMag > _moveSpeed) {
                _rigidBody.velocity = new Vector2(
                    Mathf.Sign(_rigidBody.velocity.x) * _moveSpeed,
                    _rigidBody.velocity.y);
            }
        }

        private void OnHatThrown(PlayerThrow.HatThrown _) => 
            _isEnabled = true;

        private void OnHatCaught(Hat.Hat.CaughtEvent _) =>
            _isEnabled = false;

        private void OnMove(InputValue input) =>
            _moveInput = input.Get<Vector2>();

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("World")) {
                _isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.CompareTag("World")) {
                _isGrounded = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Enemy")) {
                _hurtAudio.Play();
            }
        }
    }
}
