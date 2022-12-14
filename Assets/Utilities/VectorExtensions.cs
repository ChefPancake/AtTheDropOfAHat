using UnityEngine;

namespace DropOfAHat.Utilities {
    public static class VectorExtensions {
        public static Vector3 WithZeroZ(this Vector3 vec) =>
            vec.WithZ(0);

        public static Vector3 WithZ(this Vector3 vec, float z) =>
            new Vector3(vec.x, vec.y, z);

        public static Vector2 AsVector2(this Vector3 vec) =>
            new Vector2(vec.x, vec.y);
    }
}
