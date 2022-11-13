using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DropOfAHat.Player {
    public class Player : MonoBehaviour {
        [SerializeField]
        private bool _isEnabled = false;

        [SerializeField]
        private float _moveSpeed = 15f;
        [SerializeField]
        private float _jumpSpeed = 10f;

        [SerializeField]
        private float _groundAccel = 10f;
        [SerializeField]
        private float _airAccel = 10f;

        [SerializeField]
        private float _throwSpeed = 20f;

        private bool _isGrounded = false;
        private Hat _hat;
        private Vector2 _moveInput;
        private Rigidbody2D _rigidBody;
        private Camera _mainCam;

        private void Start() {
            _mainCam = FindObjectOfType<Camera>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _hat = GetComponentInChildren<Hat>();
            _hat.Caught += DisableInput;
            if (_moveSpeed == 0f) {
                throw new ArgumentException($"{_moveSpeed} cannot be 0");
            }
        }

        private void DisableInput() =>
            _isEnabled = false;

        private void OnMove(InputValue input) =>
            _moveInput = input.Get<Vector2>();

        private void OnJump() {
            if (_isEnabled) {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpSpeed);
            }
        }

        private void Update() {
            var accel = _isGrounded 
                ? _groundAccel 
                : _airAccel;
            var forceVecX = _isEnabled 
                ? _moveInput.x * accel
                : 0f;
            var forceVec = new Vector2(
                forceVecX,
                0f);
            _rigidBody.AddForce(forceVec);
        }
    
        private void LateUpdate() {
            var horiVelMag = Mathf.Abs(_rigidBody.velocity.x);
            if (horiVelMag > _moveSpeed) {
                _rigidBody.velocity = new Vector2(
                    Mathf.Sign(_rigidBody.velocity.x) * _moveSpeed,
                    _rigidBody.velocity.y);
            }
        }

        private void OnThrow() {
            if (!_isEnabled) {
                _isEnabled = true;   
                var mousePos = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                var throwVec = new Vector3(
                    mousePos.x - _hat.transform.position.x,
                    mousePos.y - _hat.transform.position.y,
                    0f).normalized * _throwSpeed;
                throwVec.x += _rigidBody.velocity.x;
                throwVec.y += _rigidBody.velocity.y;
                _hat.Throw(throwVec);
            }
        }

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
