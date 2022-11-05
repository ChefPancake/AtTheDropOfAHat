using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour {
    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private bool _isOnPlayer = true;

    private void Start() {

    }

    private void Update() {
        if (_isOnPlayer) {
            transform.position = _player.transform.position + _offset;
        }
    }    
}
