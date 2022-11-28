using System;
using DropOfAHat.Game;
using DropOfAHat.Hat;
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
        [SerializeField]
        private float _coyoteTimeLengthSeconds = 0.25f;
        [SerializeField]
        private AudioClip _jumpStartSound;
        [SerializeField]
        private AudioClip _jumpLandSound;

        private bool _isEnabled = false;
        private bool _isGrounded = false;
        private bool _isInAir = true;
        private float _jumpInput;
        private float _floatTime;
        private float _coyoteTime;

        private Rigidbody2D _rigidBody;
        private GameEvents _events;
        private Animator _animator;
        private AudioSource _audio;

        void Start() {
            _rigidBody = GetComponent<Rigidbody2D>();
            _events = FindObjectOfType<GameEvents>();
            _animator = GetComponentInChildren<Animator>();
            _audio = GetComponent<AudioSource>();
            if (_floatLengthSeconds == 0f) {
                throw new ArgumentException($"{nameof(_floatLengthSeconds)} cannot be 0");
            }
            _events.Subscribe<PlayerThrow.HatThrown>(OnHatThrown);
            _events.Subscribe<Hat.Hat.CaughtEvent>(OnHatCaught);
            _coyoteTime = _coyoteTimeLengthSeconds;
        }

        void Update() {
            _floatTime += Time.deltaTime;
            if (!_isGrounded && !_isInAir) {
                _coyoteTime += Time.deltaTime;
                if (_coyoteTime > _coyoteTimeLengthSeconds) {
                    SetIsInAir(!_isGrounded);
                }
            }
            if (_isEnabled && _jumpInput > 0) {    
                if (!_isInAir) {
                    _audio.PlayOneShot(_jumpStartSound);
                    _rigidBody.velocity = new Vector2(
                        _rigidBody.velocity.x,
                        _jumpSpeed);
                    _floatTime = 0;
                    _coyoteTime = _coyoteTimeLengthSeconds;
                    SetIsInAir(true);
                    _isGrounded = false;
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

        private void OnHatCaught(Hat.Hat.CaughtEvent _) =>
            _isEnabled = false;

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("World")) {
                if (_isInAir) {
                    _audio.PlayOneShot(_jumpLandSound);
                }
                _isGrounded = true;
                SetIsInAir(false);
                _coyoteTime = 0f;
            }
        }

        private void OnCollisionStay2D(Collision2D other) {
            if (other.gameObject.CompareTag("World")) {
                _coyoteTime = 0f;
            }
        }

        private void SetIsInAir(bool val) {
            _isInAir = val;
            _animator.SetBool(IS_IN_AIR_ANIMATION_STATE, val);
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.CompareTag("World")) {
                _isGrounded = false;
            }
        }
    }
}