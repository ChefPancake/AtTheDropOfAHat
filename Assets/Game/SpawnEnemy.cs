using DropOfAHat.Utilities;
using UnityEngine;

namespace DropOfAHat.Game {
    public class SpawnEnemy : MonoBehaviour {
        [SerializeField]
        private GameObject _enemyPrefab;
        [SerializeField]
        private bool _isEnabled;
    
        private GameEvents _events;
        private uint _spawnOrdinal;

        private GameObject _instance;

        private void Start() {
            _events = FindObjectOfType<GameEvents>();
            _spawnOrdinal = 
                transform.parent?
                .GetComponentInChildren<Checkpoint>()?
                .Ordinal ?? 0;
            Debug.Log($"Spawn Ordinal {_spawnOrdinal}");
            _events.Subscribe<GameManager.LevelStartEvent>(OnLevelLoad);
            _events.Subscribe<GameManager.LevelEndedEvent>(OnLevelEnd);
        }

        private void OnLevelLoad(GameManager.LevelStartEvent started) {
            if (_isEnabled && started.CheckpointOrdinal == _spawnOrdinal) {
                _instance = Instantiate(_enemyPrefab, transform.position.WithZeroZ(), Quaternion.identity);
            }
        }

        private void OnLevelEnd(GameManager.LevelEndedEvent _) {
            if (_instance) {
                Destroy(_instance);
            }
        }
    }
}
