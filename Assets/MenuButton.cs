using UnityEngine;

public class MenuButton : MonoBehaviour {
    [SerializeField]
    private string _eventName;
    [SerializeField]
    private bool _isEnabled;

    private GameStart _gameStart;

    private void Start() {
        _gameStart = GetComponentInParent<GameStart>();
    }

    public void Disable() => _isEnabled = false;

    public void Enable() => _isEnabled = true;

    private void OnMouseDown() {
        if (_isEnabled) {
            _gameStart.ButtonClick(_eventName);
        }
    }
}
