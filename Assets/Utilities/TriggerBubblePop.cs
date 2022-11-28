using UnityEngine;

namespace DropOfAHat.Utilities {
    public class TriggerBubblePop : MonoBehaviour {
        [SerializeField]
        private AudioClip _popSound;

        private ParticleSystem _particles;
        private AudioSource _popAudio;
        
        private void Start() {
            _particles = GetComponent<ParticleSystem>();
            _popAudio = GetComponentInParent<AudioSource>();
            _particles.Stop();
            _particles.Clear();
        }

        public void StartParticles() {
            _particles.Play();
            _popAudio.PlayOneShot(_popSound);
        }

        private void StopParticles() =>
            _particles.Stop();
    }
}
