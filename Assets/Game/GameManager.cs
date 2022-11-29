using UnityEngine;
using DropOfAHat.Enemy;
using DropOfAHat.Player;
using DropOfAHat.Utilities;
using System.Collections;
using System;
using System.Linq;

namespace DropOfAHat.Game {
    public class GameManager : MonoBehaviour {
        [SerializeField]
        private float _delayAfterLoseSeconds = 1.5f;
        [SerializeField]
        private GameObject _player;

        private GameEvents _events;
        private LevelManager _levels;
        private AudioSource _musicAudio;
        
        private uint _lastCheckpointOrdinal;

        private void Start() {
            _player = _player ?? FindObjectOfType<PlayerMovement>().gameObject;
            _events = FindObjectOfType<GameEvents>();
            _levels = FindObjectOfType<LevelManager>();
            _musicAudio = GetComponent<AudioSource>();
            _events.Subscribe<Hat.Hat.DroppedEvent>(OnHatDropped);
            _events.Subscribe<LevelStart.LevelLoadedEvent>(OnLevelLoaded);
            _events.Subscribe<HitBroadcast.HitEvent>(OnHitBroadcast);
        }

        private void OnHitBroadcast(HitBroadcast.HitEvent hit) {
            if (hit.Sender.CompareTag("LevelEnd")) {
                _events.Send<LevelEndedEvent>(LevelEndedEvent.Instance);
                _lastCheckpointOrdinal = 0;
                _levels.LoadNextLevel();
            } else if (hit.Sender.TryGetComponent<Checkpoint>(out var checkpoint)) {
                _lastCheckpointOrdinal = checkpoint.Ordinal;
                Debug.Log($"Hit Checkpoint {_lastCheckpointOrdinal}");
            }
        }

        private void OnHatDropped(Hat.Hat.DroppedEvent _) =>
            StartCoroutine(DelayThenRun(_delayAfterLoseSeconds, ReloadLevel));

        private void ReloadLevel() {
            foreach (var enemy in FindObjectsOfType<EnemyHunting>()) {
                Debug.Log($"Destroying {enemy.gameObject.name}");
                Destroy(enemy.gameObject);
            }
            _levels.ReloadCurrentLevel();
        }

        private void OnLevelLoaded(LevelStart.LevelLoadedEvent loadedEvent) {
            _musicAudio.Stop();
            _player.transform.position =
                FindObjectsOfType<Checkpoint>()
                    .FirstOrDefault(x => x.Ordinal.Equals(_lastCheckpointOrdinal))?
                    .transform.position.WithZeroZ()
                ?? loadedEvent.Start.transform.position.WithZeroZ();
            _musicAudio.Play();
        }

        private IEnumerator DelayThenRun(float seconds, Action action) {
            yield return new WaitForSeconds(seconds);
            action();
        }

        public struct LevelEndedEvent {
            public static LevelEndedEvent Instance { get; } = 
                new LevelEndedEvent();
        }
    }
}
