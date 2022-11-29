using DropOfAHat.Game;
using UnityEngine;

namespace DropOfAHat.Player {
    public class PlayerReset : MonoBehaviour {
        private GameEvents _events;
        private Rigidbody2D _rigidBody;

        private void Start() {
            _rigidBody = GetComponent<Rigidbody2D>();
            _events = FindObjectOfType<GameEvents>();
            _events.Subscribe<GameManager.LevelEndedEvent>(OnLevelEnd);
        }

        private void OnLevelEnd(GameManager.LevelEndedEvent _) {
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.angularVelocity = 0f;
        }
    }
}
