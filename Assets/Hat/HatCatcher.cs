using UnityEngine;

namespace DropOfAHat.Hat {
    public class HatCatcher : MonoBehaviour {
        [SerializeField]
        private bool _canThrowFrom = false;

        public bool CanThrowFrom => _canThrowFrom;
    }
}
