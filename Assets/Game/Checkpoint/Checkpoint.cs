using UnityEngine;

namespace DropOfAHat.Game {
    public class Checkpoint : MonoBehaviour {
        private const string POPPED_ANIMATION_STATE = "Popped";
        
        [SerializeField]
        private uint _ordinal;
        [SerializeField]
        private AudioClip _popSound;

        private Animator _animator;
        private ParticleSystem _particles;
        private AudioSource _audio;
        private bool _popped = false;

        public uint Ordinal => _ordinal;

        private void Start() {
            _animator = GetComponent<Animator>();
            _particles = GetComponent<ParticleSystem>();
            _audio = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!_popped && other.gameObject.CompareTag("Player")) {
                Pop();
            }
        }

        public void Pop() {
            _popped = true;
            _animator.SetBool(POPPED_ANIMATION_STATE, true);
            _audio.PlayOneShot(_popSound);
            _particles.Play();
        }

        public void Reset() {
            _popped = false;
            _animator.SetBool(POPPED_ANIMATION_STATE, false);
        }
    }
}
