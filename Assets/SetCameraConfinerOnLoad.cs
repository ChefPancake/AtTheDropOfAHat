using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SetCameraConfinerOnLoad : MonoBehaviour {
    private void Start() {
        FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = 
            GetComponent<PolygonCollider2D>();
        Debug.Log("Loaded border");
    }
}
