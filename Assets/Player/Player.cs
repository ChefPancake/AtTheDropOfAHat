using UnityEngine;
using UnityEngine.InputSystem;

namespace DropOfAHat.Player {
    public class Player : MonoBehaviour {
        [SerializeField]
        private bool _isEnabled = true;

        [SerializeField]
        private float _moveSpeed = 15f;

        private Hat _hat;
        private Vector2 _moveInput;
        private Rigidbody2D _rigidBody;
        private Camera _mainCam;

        private void Start() {
            _mainCam = FindObjectOfType<Camera>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _hat = GetComponentInChildren<Hat>();
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
    
        private void OnThrow() {
            var mousePos = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var throwVec = new Vector3(
                mousePos.x - _hat.transform.position.x,
                mousePos.y - _hat.transform.position.y,
                0f).normalized;
            _hat.Throw(throwVec);
        }
    }
}
