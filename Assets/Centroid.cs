using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Centroid : MonoBehaviour {
    [SerializeField]
    private GameObject[] _objects;

    private void Update() {
        var averageX = _objects.Average(x => x.transform.position.x);
        var averageY = _objects.Average(x => x.transform.position.y);
        transform.position = new Vector3(averageX, averageY);
    }
}
