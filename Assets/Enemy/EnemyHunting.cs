using DropOfAHat.Game;
using System;
using UnityEngine;

namespace DropOfAHat.Enemy {
    public class EnemyHunting : MonoBehaviour {
        [SerializeField]
        private float _maxSpeed;
        [SerializeField]
        private float _moveForce;

        private Rigidbody2D _rigidBody;
        private Hat.Hat _hat;
        private GameEvents _events;

        private void Start() {
            _hat = FindObjectOfType<Hat.Hat>()
                ?? throw new ArgumentNullException(nameof(_hat), "Must have Hat in the scene");
            _rigidBody = GetComponent<Rigidbody2D>();
            _events = FindObjectOfType<GameEvents>();
            Debug.Log("Enemy Spawned");
        }

        private void OnDestroy() {
            Debug.Log("Enemy Destroyed");
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

        private void FixedUpdate() {
            transform.localScale =
                new Vector3(
                    _rigidBody.velocity.x > 0f 
                        ? 1.0f 
                        : -1.0f,
                    1f,
                    1f);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                _events.Send<Hat.Hat.DroppedEvent>(new Hat.Hat.DroppedEvent());
            }
        }
    }
}
