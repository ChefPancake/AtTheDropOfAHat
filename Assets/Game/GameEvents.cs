using UnityEngine;
using DropOfAHat.Events;
using System;

namespace DropOfAHat.Game {
    public class GameEvents : MonoBehaviour {
        [SerializeField]
        private bool _logEvents;

        private IEventQueue _eventQueue;

        private void Awake() {
            _eventQueue = new EventQueue();
        }

        public void Subscribe<T>(Action<T> action) =>
            _eventQueue.Subscribe(action);

        public void Send<T>(T payload) {
            _eventQueue.Send(payload);
            if (_logEvents) {
                Debug.Log(payload);
            }
        }

        void Update() =>
            _eventQueue.ProcessAllEvents();
    }
}