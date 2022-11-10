using UnityEngine;
using UnityEngine.SceneManagement;
using DropOfAHat.Events;

namespace DropOfAHat.Game {
    public class GameManager : MonoBehaviour {
        private GameEvents _events;

        private void Start() {
            _events = FindObjectOfType<GameEvents>();
            _events.Subscribe<Hat.DroppedEvent>(OnHatDropped);
            _events.Subscribe<LevelEnd.HitEvent>(OnLevelEndHit);
        }

        private void OnHatDropped(Hat.DroppedEvent _) => 
            EndGame();

        private void OnLevelEndHit(LevelEnd.HitEvent hitEvent) {
            if (hitEvent.Tag == "Player") {
                EndGame();
            }
        }

        private void EndGame() =>
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
