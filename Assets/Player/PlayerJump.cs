using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DropOfAHat.Player {
    public class PlayerJump : MonoBehaviour {
        private const string IS_IN_AIR_ANIMATION_STATE = "IsInAir";
        private const string Y_VEL_ANIMATION_STATE = "YVel";

        [SerializeField]
        private float _jumpSpeed = 10f;
        [SerializeField]
        private float _floatForce = 1f;
        [SerializeField]
        private float _floatLengthSeconds = 1f;

        private bool _isEnabled = false;
        private bool _isGrounded = false;
        private float _jumpInput;
        private float _floatTime;

        private Rigidbody2D _rigidBody;
        private GameEvents _events;
        private Animator _animator;

        void Start() {
            _rigidBody = GetComponent<Rigidbody2D>();
            _events = FindObjectOfType<GameEvents>();
            _animator = GetComponentInChildren<Animator>();
            if (_floatLengthSeconds == 0f) {
                throw new ArgumentException($"{nameof(_floatLengthSeconds)} cannot be 0");
            }
            _events.Subscribe<PlayerThrow.HatThrown>(OnHatThrown);
            _events.Subscribe<Hat.CaughtEvent>(OnHatCaught);
        }

        void Update() {
            _floatTime += Time.deltaTime;
            if (_isEnabled && _jumpInput > 0) {
                if (_isGrounded) {
                    _rigidBody.velocity = new Vector2(
                        _rigidBody.velocity.x,
                        _jumpSpeed);
                    _floatTime = 0;
                } else if (_floatTime <= _floatLengthSeconds) {
                    var t = _floatTime / _floatLengthSeconds;
                    var lerpFloatForce = Mathf.Lerp(_floatForce, 0f, t);
                    _rigidBody.AddForce(new Vector2(0f, lerpFloatForce));
                }
            }
            _animator.SetFloat(Y_VEL_ANIMATION_STATE, _rigidBody.velocity.y);
        }

        private void OnJump(InputValue input) =>
            _jumpInput = input.Get<float>();

        private void OnHatThrown(PlayerThrow.HatThrown _) =>
            _isEnabled = true;

        private void OnHatCaught(Hat.CaughtEvent _) =>
            _isEnabled = false;

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("World")) {
                _isGrounded = true;
                _animator.SetBool(IS_IN_AIR_ANIMATION_STATE, false);
            }
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.CompareTag("World")) {
                _isGrounded = false;
                _animator.SetBool(IS_IN_AIR_ANIMATION_STATE, true);
            }
        }
    }
}