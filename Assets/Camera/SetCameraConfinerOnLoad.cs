using Cinemachine;
using UnityEngine;

namespace DropOfAHat.Camera {
    public class SetCameraConfinerOnLoad : MonoBehaviour {
        private void Start() {
            FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D =
                GetComponent<PolygonCollider2D>();
        }
    }
}