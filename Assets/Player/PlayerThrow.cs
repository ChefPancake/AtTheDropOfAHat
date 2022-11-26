using DropOfAHat.Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DropOfAHat.Player {
    public class PlayerThrow : MonoBehaviour {
        [SerializeField]
        private float _throwSpeed = 20f;
        
        private bool _isHoldingHat = true;
        private Rigidbody2D _rigidBody;
        private UnityEngine.Camera _mainCam;
        private GameEvents _events;

        private void Start() {
            _mainCam = FindObjectOfType<UnityEngine.Camera>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _events = FindObjectOfType<GameEvents>();
            _events.Subscribe<Hat.Hat.CaughtEvent>(OnHatCaught);
        }

        private void OnThrow() {
            if (_isHoldingHat) {
                _isHoldingHat = false;
                var mousePos = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                var throwVec = new Vector3(
                    mousePos.x - transform.position.x,
                    mousePos.y - transform.position.y,
                    0f).normalized * _throwSpeed;
                throwVec.x += _rigidBody.velocity.x;
                throwVec.y += _rigidBody.velocity.y;
                _events.Send<HatThrown>(new HatThrown(throwVec));
            }
        }

        private void OnHatCaught(Hat.Hat.CaughtEvent _) =>
            _isHoldingHat = true;

        public struct HatThrown {
            public Vector3 ThrowVec { get; }

            public HatThrown(Vector3 throwVec) => 
            ThrowVec = throwVec;
        }
    }
}