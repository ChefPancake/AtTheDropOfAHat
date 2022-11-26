using DropOfAHat.Utilities;
using UnityEngine;

namespace DropOfAHat.Game {
    public class TrackMousePosition : MonoBehaviour {
        private const float PIXELS_PER_UNIT = 20f;

        [SerializeField]
        private GameObject _toTrack;
        [SerializeField]
        private float _multiplier = 1f;

        private Vector3 _centerPos;

        private void Start() {
            var camera = FindObjectOfType<UnityEngine.Camera>();
            _centerPos = new Vector3(
                camera.pixelWidth / 2,
                camera.pixelHeight / 2,
                0f);
        }

        private void Update() {
            transform.position = MousePosToWorldPos(Input.mousePosition).WithZeroZ();
        }

        private Vector3 MousePosToWorldPos(Vector3 mousePos) =>
            _toTrack.transform.position + (mousePos - _centerPos) / PIXELS_PER_UNIT * _multiplier;
    }
}