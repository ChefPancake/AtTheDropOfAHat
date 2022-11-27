using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParticles : MonoBehaviour {
    private ParticleSystem _particles;
    
    private void Start() {
        _particles = GetComponent<ParticleSystem>();
        _particles.Stop();
        _particles.Clear();
    }

    public void StartParticles() =>
        _particles.Play();

    private void StopParticles() =>
        _particles.Stop();
}
