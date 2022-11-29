using UnityEngine;

namespace DropOfAHat.Player {
    public class TriggerFootsteps : MonoBehaviour {
        [SerializeField]
        private AudioClip _footstepClipLeft;
        [SerializeField]
        private AudioClip _footstepClipRight;

        private AudioSource _audio;

        private void Start() {
            _audio = GetComponentInParent<AudioSource>();
        }

        public void PlayFootstepLeft() {
            _audio.PlayOneShot(_footstepClipLeft);
        }

        public void PlayFootstepRight() {
            _audio.PlayOneShot(_footstepClipRight);
        }
    }
}
