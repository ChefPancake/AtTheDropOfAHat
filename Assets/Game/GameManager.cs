using UnityEngine;
using DropOfAHat.Enemy;
using DropOfAHat.Player;
using DropOfAHat.Utilities;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace DropOfAHat.Game {
    public class GameManager : MonoBehaviour {
        [SerializeField]
        private float _delayAfterLoseSeconds = 1.5f;
        [SerializeField]
        private GameObject _player;
        
        [SerializeField]
        private GameObject[] _objectsToDisableOnPause;

        [SerializeField]
        private string _endSceneName;

        private GameEvents _events;
        private LevelManager _levels;
        private AudioSource _musicAudio;
        private GameTimer _timer;
        
        private uint _lastCheckpointOrdinal;

        private bool _playerAtEnd = false;
        private bool _hatAtEnd = false;
        private bool _resettingLevel = false;
        private bool LevelComplete => _playerAtEnd && _hatAtEnd;

        private void Awake() {
            _musicAudio = GetComponent<AudioSource>();
        }

        private void Start() {
            _player = _player ?? FindObjectOfType<PlayerMovement>().gameObject;
            _events = FindObjectOfType<GameEvents>();
            _levels = FindObjectOfType<LevelManager>();
            _timer = FindObjectOfType<GameTimer>();
            _events.Subscribe<Hat.Hat.DroppedEvent>(OnHatDropped);
            _events.Subscribe<LevelStart.LevelLoadedEvent>(OnLevelLoaded);
            _events.Subscribe<HitBroadcast.HitEvent>(OnHitBroadcast);
            _events.Subscribe<HitBroadcast.LeftEvent>(OnLeftBroadcast);
        }

        public void PauseGame() {
            foreach (var obj in _objectsToDisableOnPause) {
                obj.SetActive(false);
            }
        }

        public void StartGame() {
            _musicAudio.Play();
            foreach (var obj in _objectsToDisableOnPause) {
                obj.SetActive(true);
            }
        }

        private void OnLeftBroadcast(HitBroadcast.LeftEvent left) {
            if (left.Sender.CompareTag("LevelEnd") && left.Collider.CompareTag("Player")) {
                _playerAtEnd = false;
            }
        }

        private void OnHitBroadcast(HitBroadcast.HitEvent hit) {
            if (hit.Sender.CompareTag("LevelEnd")) {
                if (hit.Collider.gameObject.CompareTag("Hat")) {
                    _hatAtEnd = true;
                } else if (hit.Collider.gameObject.CompareTag("Player")) {
                    _playerAtEnd = true;
                }
                if (LevelComplete) {
                    _events.Send<LevelEndedEvent>(LevelEndedEvent.Instance);
                    _lastCheckpointOrdinal = 0;
                    if (_levels.TryLoadNextLevel()) {
                        _playerAtEnd = false;
                        _hatAtEnd = false;
                    } else {
                        LoadEndOfGame();
                    }
                }
            } else if (hit.Sender.TryGetComponent<Checkpoint>(out var checkpoint)) {
                _lastCheckpointOrdinal = checkpoint.Ordinal;
                Debug.Log($"Hit Checkpoint {_lastCheckpointOrdinal}");
            }
        }

        private void LoadEndOfGame() {
            _timer.StopTimer();
            PauseGame();
            SceneManager.LoadScene(_endSceneName, LoadSceneMode.Additive);
        }

        private void OnHatDropped(Hat.Hat.DroppedEvent _) {
            if (!_resettingLevel) {
                _resettingLevel = true;
                StartCoroutine(DelayThenRun(_delayAfterLoseSeconds, ReloadLevel));
            }
        }

        private void ReloadLevel() {
            foreach (var enemy in FindObjectsOfType<EnemyHunting>()) {
                Destroy(enemy.gameObject);
            }
            _playerAtEnd = false;
            _hatAtEnd = false;
            _resettingLevel = false;
            _levels.ReloadCurrentLevel();
        }

        private void OnLevelLoaded(LevelStart.LevelLoadedEvent loadedEvent) {
            try {

            var checkpoint = 
                FindObjectsOfType<Checkpoint>()
                .FirstOrDefault(x => x.Ordinal.Equals(_lastCheckpointOrdinal));
            checkpoint?.Reset();            
            _player.transform.position =
                checkpoint?.transform.position.WithZ(_player.transform.position.z)
                    ?? loadedEvent.Start.transform.position.WithZeroZ();
            if (_player.TryGetComponent<PlayerDeath>(out var death)) {
                death.Revive();
            }
            _events.Send<LevelStartEvent>(new LevelStartEvent(_lastCheckpointOrdinal));
            }
            catch (Exception ex) {
                Debug.LogError($"{ex.Message}: {ex.StackTrace}");
            }
        }

        private IEnumerator DelayThenRun(float seconds, Action action) {
            yield return new WaitForSeconds(seconds);
            action();
        }

        public struct LevelEndedEvent {
            public static LevelEndedEvent Instance { get; } = 
                new LevelEndedEvent();
        }

        public struct LevelStartEvent {
            public uint CheckpointOrdinal { get; }

            public LevelStartEvent(uint checkpointOrdinal) =>
                CheckpointOrdinal = checkpointOrdinal;
        }
    }
}
