using UnityEngine;
using UnityEngine.SceneManagement;
using DropOfAHat.Events;

namespace DropOfAHat.Game {
    public class GameManager : MonoBehaviour {
        
        [SerializeField]
        private TriggerListener _listener;

        private void Start() {
            _listener.Triggered += OnListenerTriggered;
        }
        
        private void OnListenerTriggered() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
