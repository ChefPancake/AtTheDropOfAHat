using UnityEngine;

namespace DropOfAHat.Utilities {
    public static class VectorExtensions {
        public static Vector3 WithZeroZ(this Vector3 vec) =>
            new Vector3(vec.x, vec.y, 0f);
    }
}
