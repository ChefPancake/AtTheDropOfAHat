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

        private void Start() {
            var currentSceneName = _levelScenes[_currentSceneIndex];
            if (!ActiveScenes().Any(x => x.name.Equals(currentSceneName))) {
                SceneManager.LoadScene(_levelScenes[_currentSceneIndex], LoadSceneMode.Additive);
            }
        }

        public void LoadNextLevel() {
            var nextSceneIndex = (_currentSceneIndex + 1) % _levelScenes.Length;
            SceneManager.UnloadSceneAsync(_levelScenes[_currentSceneIndex]);
            SceneManager.LoadScene(_levelScenes[nextSceneIndex], LoadSceneMode.Additive);
            _currentSceneIndex = nextSceneIndex;
        }

        public void ReloadCurrentLevel() {
            SceneManager.UnloadSceneAsync(_levelScenes[_currentSceneIndex]);
            SceneManager.LoadScene(_levelScenes[_currentSceneIndex], LoadSceneMode.Additive);
        }

        private IEnumerable<Scene> ActiveScenes() =>
            Enumerable.Range(0, SceneManager.sceneCount)
            .Select(x => SceneManager.GetSceneAt(x));
    }
}