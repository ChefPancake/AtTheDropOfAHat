using System.Linq;
using UnityEngine;

namespace DropOfAHat.Utilities {
    public class Centroid : MonoBehaviour {
        [SerializeField]
        private GameObject[] _objects;

        private void Update() {
            var averageX = _objects.Average(x => x.transform.position.x);
            var averageY = _objects.Average(x => x.transform.position.y);
            transform.position = new Vector3(averageX, averageY);
        }
    }
}
