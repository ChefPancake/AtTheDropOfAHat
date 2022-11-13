using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DropOfAHat.Player {
    public class PlayerMovement : MonoBehaviour {
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

        private void Start() {
            _rigidBody = GetComponent<Rigidbody2D>();
            _events = FindObjectOfType<GameEvents>();
            _events.Subscribe<PlayerThrow.HatThrown>(OnHatThrown);
            _events.Subscribe<Hat.Caught>(OnHatCaught);
        }

        private void Update() {
            if (_isEnabled) {
                var accel = _isGrounded 
                    ? _groundAccel 
                    : _airAccel;
                var forceVec = new Vector2(
                    _moveInput.x * accel,
                    0f);
                _rigidBody.AddForce(forceVec);
            }
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

        private void OnHatCaught(Hat.Caught _) =>
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
    }
}
