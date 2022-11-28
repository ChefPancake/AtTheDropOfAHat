using DropOfAHat.Utilities;
using UnityEngine;

namespace DropOfAHat.Game {
    public class SpawnEnemy : MonoBehaviour {
        [SerializeField]
        private GameObject _enemyPrefab;
    
        private GameEvents _events;

        private GameObject _instance;

        private void Start() {
            _events = FindObjectOfType<GameEvents>();
            _events.Subscribe<LevelStart.LevelLoadedEvent>(OnLevelLoad);
            _events.Subscribe<LevelEnd.HitEvent>(OnLevelEnd);
        }

        private void OnLevelLoad(LevelStart.LevelLoadedEvent _) =>
            _instance = Instantiate(_enemyPrefab, transform.position.WithZeroZ(), Quaternion.identity);

        private void OnLevelEnd(LevelEnd.HitEvent _) =>
            Destroy(_instance);
    }
}
