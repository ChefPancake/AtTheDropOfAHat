using DropOfAHat.Game;
using DropOfAHat.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DropOfAHat.Player {
    public class PlayerThrow : MonoBehaviour {
        private float THROW_COEFFICIENT = 2.5f;

        [SerializeField]
        private float _maxThrowSpeed = 80f;
        
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
                var directionVec = (mousePos - transform.position).WithZeroZ();
                var throwVec = 
                    Vector3.ClampMagnitude(
                        directionVec * THROW_COEFFICIENT,
                        _maxThrowSpeed);
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