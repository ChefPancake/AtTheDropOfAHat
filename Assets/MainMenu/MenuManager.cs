using UnityEngine;
using UnityEngine.SceneManagement;

namespace DropOfAHat.Menu {
    public class MenuManager : MonoBehaviour {
        [SerializeField]
        private string _loadSceneOnPlay;

        public void Play() {
            SceneManager.LoadScene(_loadSceneOnPlay);
        }

        public void Quit() {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
