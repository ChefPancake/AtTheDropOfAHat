using UnityEngine;

namespace DropOfAHat.Game {
    public class Checkpoint : MonoBehaviour {
        [SerializeField]
        private uint _ordinal;

        public uint Ordinal => _ordinal;
    }
}
