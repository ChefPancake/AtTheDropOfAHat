using UnityEngine;

public class PlayerDeath : MonoBehaviour {
    private const string IS_DEAD_ANIMATION_STATE = "IsDead";

    [SerializeField]
    private AudioClip _deathSound;

    private Animator _animator;
    private ParticleSystem _particles;
    private Rigidbody2D _rigidbody;
    private AudioSource _audio;

    private bool _isDead = false;

    private void Start() {
        _animator = GetComponentInChildren<Animator>();
        _particles = GetComponentInChildren<ParticleSystem>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
    }

    public void Kill() {
        if (!_isDead) {
            _isDead = true;
            _rigidbody.simulated = false;
            _particles.Play();
            _animator.SetBool(IS_DEAD_ANIMATION_STATE, true);
            _audio.PlayOneShot(_deathSound);
        }
    }

    public void Revive() {
        if (_isDead) {
            _isDead = false;
            _rigidbody.simulated = true;
            _particles.Stop();
            _animator.SetBool(IS_DEAD_ANIMATION_STATE, false);
        }
    }
}
