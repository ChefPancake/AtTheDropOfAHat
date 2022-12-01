using UnityEngine;

namespace DropOfAHat.Hat {
    public class HatCatcher : MonoBehaviour {
        [SerializeField]
        private bool _canThrowFrom = false;
        [SerializeField]
        private Vector3 _hatOffset = Vector3.zero;

        public Vector3 HatOffset => _hatOffset;

        public bool CanThrowFrom => _canThrowFrom;
    }
}
