using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {
    private GameEvents _events;

    private void Start() {
        _events = FindObjectOfType<GameEvents>();
    }

    private void OnTriggerEnter2D(Collider2D other) =>
        _events.Send(new HitEvent(other.gameObject.tag));

    public struct HitEvent {
        public string Tag { get; }

        public HitEvent(string tag) =>
            Tag = tag;
    }
}
