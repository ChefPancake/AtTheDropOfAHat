using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DropOfAHat.Events;
using System;

public class GameEvents : MonoBehaviour {
    private IEventQueue _eventQueue;
    
    private void Awake() {
        _eventQueue = new EventQueue();
    }

    public void Subscribe<T>(Action<T> action) =>
        _eventQueue.Subscribe(action);

    public void Send<T>(T payload) =>
        _eventQueue.Send(payload);

    void Update() =>
        _eventQueue.ProcessAllEvents();
}
