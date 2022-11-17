using UnityEngine;
using UnityEngine.SceneManagement;
using DropOfAHat.Events;
using DropOfAHat.Player;
using System;

namespace DropOfAHat.Game {
    public class GameManager : MonoBehaviour {
        private GameEvents _events;
        private GameObject _player;

        private void Start() {
            _player = FindObjectOfType<PlayerMovement>().gameObject;
            _events = FindObjectOfType<GameEvents>();
            _events.Subscribe<Hat.DroppedEvent>(OnHatDropped);
            _events.Subscribe<LevelStart.LevelLoadedEvent>(OnLevelLoaded);
        }

        private void OnHatDropped(Hat.DroppedEvent _) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        }

        private void OnLevelLoaded(LevelStart.LevelLoadedEvent loadedEvent) =>
            _player.transform.position =
                loadedEvent.Start.transform.position.WithZeroZ();
    }
}
