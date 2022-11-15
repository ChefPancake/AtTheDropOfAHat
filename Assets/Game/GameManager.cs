using UnityEngine;
using UnityEngine.SceneManagement;
using DropOfAHat.Events;

namespace DropOfAHat.Game {
    public class GameManager : MonoBehaviour {
        private GameEvents _events;

        private void Start() {
            _events = FindObjectOfType<GameEvents>();
            _events.Subscribe<Hat.DroppedEvent>(OnHatDropped);
        }

        private void OnHatDropped(Hat.DroppedEvent _) => 
            EndGame();

        private void EndGame() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        }
    }
}
