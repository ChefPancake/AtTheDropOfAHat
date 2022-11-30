using UnityEngine;
using TMPro;

namespace DropOfAHat.Game {
    public class GameTimer : MonoBehaviour {
        private enum TimerStates {
            Stopped,
            Paused,
            Running
        }
        
        private float _elapsedTime = 0;
        private TextMeshProUGUI _text;
        private TimerStates _state = TimerStates.Stopped;

        private string TimeString => $"{_elapsedTime:00.000}";
        public float ElapsedTime => _elapsedTime;

        private void Start() =>
            _text = GetComponent<TextMeshProUGUI>();

        private void Update() {
            if (_state == TimerStates.Running) {
                _elapsedTime += Time.deltaTime;
                _text.text = TimeString;
            } else if (_state == TimerStates.Stopped) {
                _text.text = string.Empty;
            }
        }

        public void StartTimer() =>
            _state = TimerStates.Running;

        public void StopTimer() =>
            _state = TimerStates.Stopped;

        public void PauseTimer() => 
            _state = TimerStates.Paused;

        public void Reset() =>
            _elapsedTime = 0f;
    }
}
