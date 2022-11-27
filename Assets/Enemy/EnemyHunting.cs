using DropOfAHat.Game;
using DropOfAHat.Hat;
using System;
using UnityEngine;

public class EnemyHunting : MonoBehaviour {
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _moveForce;
    
    private Rigidbody2D _rigidBody;
    private Hat _hat;
    private GameEvents _events;

    private void Start() {
        _hat = FindObjectOfType<Hat>()
            ?? throw new ArgumentNullException(nameof(_hat), "Must have Hat in the scene");
        _rigidBody = GetComponent<Rigidbody2D>();
        _events = FindObjectOfType<GameEvents>();
    }
    
    private void Update() {
        var moveForce = 
            (_hat.transform.position - transform.position)
            .normalized 
            * _moveForce;
        _rigidBody.AddForce(moveForce);
    }

    private void LateUpdate() {
        _rigidBody.velocity = 
            Vector2.ClampMagnitude(_rigidBody.velocity, _maxSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            _events.Send<Hat.DroppedEvent>(new Hat.DroppedEvent());
        }
    }
}
