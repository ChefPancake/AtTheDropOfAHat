using UnityEngine;
using UnityEngine.SceneManagement;
using DropOfAHat.Player;
using DropOfAHat.Utilities;
using System.Collections;
using System;

namespace DropOfAHat.Game {
    public class GameManager : MonoBehaviour {
        [SerializeField]
        private float _delayAfterLoseSeconds = 1.5f;
        
        private GameEvents _events;
        private GameObject _player;

        private void Start() {
            _player = FindObjectOfType<PlayerMovement>().gameObject;
            _events = FindObjectOfType<GameEvents>();
            _events.Subscribe<Hat.Hat.DroppedEvent>(OnHatDropped);
            _events.Subscribe<LevelStart.LevelLoadedEvent>(OnLevelLoaded);
        }

        private void OnHatDropped(Hat.Hat.DroppedEvent _) {
            StartCoroutine(DelayThenRun(_delayAfterLoseSeconds, LoadFirstScene));
        }

        private void OnLevelLoaded(LevelStart.LevelLoadedEvent loadedEvent) =>
            _player.transform.position =
                loadedEvent.Start.transform.position.WithZeroZ();


        private void LoadFirstScene() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        }

        private IEnumerator DelayThenRun(float seconds, Action action) {
            yield return new WaitForSeconds(seconds);
            action();
        }
    }
}
