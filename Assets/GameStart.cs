using System.Collections;
using DropOfAHat.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer _logo;
    [SerializeField]
    private SpriteRenderer _gameCover;

    private AudioSource _music;
    private GameTimer _timer;
    private GameManager _gameManager;
    private MenuButton[] _buttons;
    
    private void Awake() {
        _logo.enabled = false;
        _gameCover.enabled = true;
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Additive);
    }

    private void Start() {
        _timer = FindObjectOfType<GameTimer>();
        _gameManager = FindObjectOfType<GameManager>();
        _buttons = GetComponentsInChildren<MenuButton>();
        _music = GetComponent<AudioSource>();
        _gameManager.PauseGame();
        _timer.StopTimer();
        StartCoroutine(nameof(LoadMenu));            
    }

    public void ButtonClick(string eventName) {
        if (eventName.Equals("StartGame")) {
            StartCoroutine(StartGame());
            _music.Stop();
        }
    }

    private IEnumerator LoadMenu() {
        yield return new WaitForSeconds(2f);
        _music.Play();
        _logo.enabled = true;
        yield return new WaitForSeconds(3f);
        Color color = _gameCover.material.color;
        for (float alpha = 1f; alpha >= 0f; alpha -= 0.02f)
        {
            color.a = alpha;
            _gameCover.material.color = color;
            yield return new WaitForSeconds(.02f);
        }
        foreach (var button in _buttons) {
            button.Enable();
        }
    }

    private IEnumerator StartGame() {
        _gameManager.StartGame();
        _timer.StartTimer();
        var unload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        var maxWaitTime = 2f;
        var waitTime = 0f;
        while (!unload.isDone && waitTime < maxWaitTime) {
            yield return new WaitForSeconds(0.1f);
            waitTime += 0.1f;
        }
    }
}
