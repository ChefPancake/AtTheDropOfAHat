using System;
using UnityEngine;

namespace DropOfAHat.Game {
    public class LevelStart : MonoBehaviour {
        private void Awake() =>
            FindObjectOfType<GameEvents>()
            .Send(new LevelLoadedEvent(this));

        public struct LevelLoadedEvent {
            public LevelStart Start { get; }

            public LevelLoadedEvent(LevelStart start) =>
                Start = start ?? throw new ArgumentNullException(nameof(start));
        }
    }
}