using DropOfAHat.Game;
using TMPro;
using UnityEngine;

public class GameEnd : MonoBehaviour {
    private void Start() {
        var time = FindObjectOfType<GameTimer>().ElapsedTime;
        var textField = GetComponent<TextMeshProUGUI>();
        textField.text = 
$@"You Win!

Time: {time:00.000}";
    }
}
