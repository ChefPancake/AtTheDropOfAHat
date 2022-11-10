using System;
using System.Collections.Generic;

namespace DropOfAHat.Events {
    public class EventQueue : IEventQueue {
        private readonly IDictionary<int, List<Subscription>> _subscriptions;
        private readonly Queue<GameEvent> _events;

        public EventQueue() {
            _subscriptions = new Dictionary<int, List<Subscription>>();
            _events = new Queue<GameEvent>();
        }

        public void Send<T>(T payload) =>
            _events.Enqueue(GameEvent.FromPayload(payload));

        public void Subscribe<T>(Action<T> callback) {
            var typeHash = typeof(T).GetHashCode();
            if (_subscriptions.TryGetValue(typeHash, out var subs)) {
                subs.Add(Subscription.FromTypedAction(callback));
            } else {
                _subscriptions.Add(
                    typeHash,
                    new List<Subscription>() {
                        Subscription.FromTypedAction(callback)
                    });
            }
        }

        public void ProcessAllEvents() {
            foreach (var gameEvent in _events.DequeueAll()) {
                var payloadTypeHash = gameEvent.PayloadType.GetHashCode();
                if (_subscriptions.TryGetValue(payloadTypeHash, out var subs)) {
                    foreach (var subscription in subs) {
                        subscription.Action(gameEvent);
                    }
                }
            }
        }

        private class GameEvent {
            public object Payload { get; }
            public Type PayloadType { get; }

            private GameEvent(object payload, Type type) {
                Payload = payload;
                PayloadType = type;
            }

            public static GameEvent FromPayload<T>(T payload) =>
                new GameEvent(payload, typeof(T));
        }

        private class Subscription {
            public Action<GameEvent> Action { get; }

            private Subscription(Action<GameEvent> action) {
                Action = action;
            }
            public static Subscription FromTypedAction<T>(Action<T> action) =>
                new Subscription(AsGameEventAction(action));

            private static Action<GameEvent> AsGameEventAction<T>(Action<T> action) =>
                (GameEvent gameEvent) => {
                    if (gameEvent.Payload is T typed) {
                        action(typed);
                    } else {
                        throw new InvalidOperationException("Reached unreachable code");
                    }
                };
        }
    }
}
