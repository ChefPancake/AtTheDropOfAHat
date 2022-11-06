using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hat : MonoBehaviour {
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Vector3 _offsetOnPlayer;
    [SerializeField]
    private bool _isOnPlayer = true;

    private bool _inAir = false;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider;

    private void Start() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        SetPhysics(!_isOnPlayer);
    }

    private void Update() {
        if (_isOnPlayer) {
            transform.position = _player.transform.position + _offsetOnPlayer;
        }
    }

    public void Catch() {
        _inAir = false;
        _isOnPlayer = true;
        SetPhysics(false);
    }

    public void Throw(Vector3 at) {
        if (_isOnPlayer) {
            _isOnPlayer = false;
            SetPhysics(true);
            _rigidBody.velocity = at;
        }
    }

    private void SetPhysics(bool enabled) {
        _collider.enabled = enabled;
        _rigidBody.simulated = enabled;
        if (enabled) {
            _rigidBody.position = transform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (!_isOnPlayer && other.gameObject.CompareTag("Player")) {
            _inAir = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (_inAir) {
            if (other.gameObject.CompareTag("Player")) {
                Catch();
            }
            if (other.gameObject.CompareTag("World")) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
