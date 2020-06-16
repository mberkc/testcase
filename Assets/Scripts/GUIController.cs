using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

    public Text scoreText;
    public Text highscoreText;
    public Text realtimeScoreText;
    public GameObject gameOverPanel;
    public GameObject holdStartText;

    public void HandleRestartButton () {
        SceneManager.LoadScene ("Game");
    }

    public void CloseHoldStartText () {
        holdStartText.SetActive (false);
    }

    public void CloseRealtimeScoreText () {
        realtimeScoreText.gameObject.SetActive (false);
    }

    public void OpenGameOverPanel () {
        gameOverPanel.SetActive (true);
    }

    public void SetHighScoreText (float score) {
        highscoreText.text = "HighestScore: " + score.ToString ("0.00");
    }

    public void SetScoreText (float score) {
        scoreText.text = score.ToString ("0.00");
    }

    public void SetRealtimeScoreText (float score) {
        realtimeScoreText.text = score.ToString ("0.00");
    }

}