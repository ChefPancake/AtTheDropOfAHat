using System;

namespace DropOfAHat.Events {
    public interface IEventQueue {
        void Subscribe<T>(Action<T> callback);
        void Send<T>(T payload);
        void ProcessAllEvents();
    }
}
