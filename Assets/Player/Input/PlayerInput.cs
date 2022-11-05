using UnityEngine;
using UnityEngine.InputSystem;

namespace DropOfAHat.Player.Input {
    public class PlayerInput : MonoBehaviour {
        [SerializeField]
        private bool _isEnabled = true;

        [SerializeField]
        private float _moveSpeed = 15f;

        private Vector2 _moveInput;
        private Rigidbody2D _rigidBody;

        private void Start() {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        private void OnMove(InputValue input) =>
            _moveInput = input.Get<Vector2>();

        private void OnJump() {
            if (_isEnabled) {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 10f);
            }
        }

        private void Update() {
            var newVel = new Vector3(
                _isEnabled ? _moveInput.x * _moveSpeed : 0f,
                _rigidBody.velocity.y,
                0f);
            _rigidBody.velocity = newVel;
        }
    }
}
