using System.Collections;
using Cinemachine;
using DropOfAHat.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer _logo;
    [SerializeField]
    private SpriteRenderer _gameCover;
    [SerializeField]
    private Color _gameCoverColor;

    private AudioSource _music;
    private GameTimer _timer;
    
    private void Awake() {
        _logo.enabled = false;
        _gameCover.material.color = _gameCoverColor;
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Additive);
    }

    private void Start() {
        _timer = FindObjectOfType<GameTimer>();
        _timer.StopTimer();
        StartCoroutine(nameof(LoadMenu));            
    }

    private IEnumerator LoadMenu() {
        yield return new WaitForSeconds(2f);
        //play the menu song

        _logo.enabled = true;
        yield return new WaitForSeconds(3f);
        Color c = _gameCover.material.color;
        for (float alpha = 1f; alpha >= 0f; alpha -= 0.02f)
        {
            c.a = alpha;
            _gameCover.material.color = c;
            yield return new WaitForSeconds(.02f);
        }
        //enable menu input
    }
}
