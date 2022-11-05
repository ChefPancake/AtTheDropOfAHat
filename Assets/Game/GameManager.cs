using UnityEngine;

namespace DropOfAHat.Game {
    public class GameManager : MonoBehaviour {
        
        [SerializeField]
        private TriggerListener _listener;

        [SerializeField]
        private bool _youWin = false;

        private void Start() {
            _listener.Triggered += OnListenerTriggered;
        }
        
        private void OnListenerTriggered() {
            _youWin = true;
        }
    }
}
