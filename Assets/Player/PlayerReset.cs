using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour {
    private GameEvents _events;
    private Rigidbody2D _rigidBody;

    private void Start() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _events = FindObjectOfType<GameEvents>();
        _events.Subscribe<LevelEnd.HitEvent>(OnLevelEndHit);
    }

    private void OnLevelEndHit(LevelEnd.HitEvent _) {
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.angularVelocity = 0f;
    }
}
