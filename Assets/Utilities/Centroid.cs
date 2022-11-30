using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DropOfAHat.Utilities {
    public class Centroid : MonoBehaviour {
        [SerializeField]
        private GameObject[] _objects;

        private void Update() {
            var averageX = _objects
                .Where(x => x && x.activeInHierarchy)
                .AverageOr(x => x.transform.position.x, transform.position.x);
            var averageY = _objects
                .Where(x => x && x.activeInHierarchy)
                .AverageOr(x => x.transform.position.y, transform.position.y);
            transform.position = new Vector3(averageX, averageY);
        }
    }

    internal static class IEnumerableExtensions {
        public static float AverageOr<T>(this IEnumerable<T> items, Func<T, float> selector, float or) =>
            items.Any() 
            ? items.Average(selector)
            : or;
    }
}


