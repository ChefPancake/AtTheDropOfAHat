using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMousePosition : MonoBehaviour {
    private const float PIXELS_PER_UNIT = 20f;

    [SerializeField]
    private GameObject _toTrack;

    [SerializeField]
    private float _multiplier = 1f;

    //We need the screen position of the mouse, the crosshairs 
    //are going to be based on that. The logical position of the 
    //crosshair would effectively be the screen position multiplied by 2,
    //assuming that origin is the center of the screen

    //for now we'll set the transform of this thing to that position

    private Vector3 _centerPos;

    private void Start() { 
        var camera = FindObjectOfType<Camera>();
        _centerPos = new Vector3(
            camera.pixelWidth / 2,
            camera.pixelHeight / 2,
            0f);
    }
    
    private void Update() {
        transform.position = MousePosToWorldPos(Input.mousePosition).WithZeroZ();
    }

    private Vector3 MousePosToWorldPos(Vector3 mousePos) =>
        _toTrack.transform.position + ((mousePos - _centerPos) / PIXELS_PER_UNIT * _multiplier);
}
