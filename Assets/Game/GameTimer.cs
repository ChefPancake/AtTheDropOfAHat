using UnityEngine;
using TMPro;

namespace DropOfAHat.Game {
    public class GameTimer : MonoBehaviour {
        private float _elapsedTime = 0;

        private string TimeString => $"{_elapsedTime:00.000}";

        private TextMeshProUGUI _text;

        private void Start() {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            _elapsedTime += Time.deltaTime;
            _text.text = TimeString;
        }
    }
}
