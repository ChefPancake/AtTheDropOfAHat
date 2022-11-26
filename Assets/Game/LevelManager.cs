using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DropOfAHat.Game {
    public class LevelManager : MonoBehaviour {
        [SerializeField]
        private string[] _levelScenes;
        [SerializeField]
        private int _currentSceneIndex;

        private GameEvents _events;
        private GameObject _player;

        private void Start() {
            _events = FindObjectOfType<GameEvents>();
            _events.Subscribe<LevelEnd.HitEvent>(OnLevelEndHit);
            var currentSceneName = _levelScenes[_currentSceneIndex];
            if (!ActiveScenes().Any(x => x.name.Equals(currentSceneName))) {
                SceneManager.LoadScene(_levelScenes[_currentSceneIndex], LoadSceneMode.Additive);
            }
        }

        private void OnLevelEndHit(LevelEnd.HitEvent hit) {
            LoadNextLevel();
        }

        private void LoadNextLevel() {
            var nextSceneIndex = (_currentSceneIndex + 1) % _levelScenes.Length;
            SceneManager.UnloadSceneAsync(_levelScenes[_currentSceneIndex]);
            SceneManager.LoadScene(_levelScenes[nextSceneIndex], LoadSceneMode.Additive);
            _currentSceneIndex = nextSceneIndex;
        }

        private IEnumerable<Scene> ActiveScenes() =>
            Enumerable.Range(0, SceneManager.sceneCount)
            .Select(x => SceneManager.GetSceneAt(x));
    }
}