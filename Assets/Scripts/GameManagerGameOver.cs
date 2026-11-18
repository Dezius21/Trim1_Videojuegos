using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManagerGameOver : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timeText;

    void Start()
    {
        int score = PlayerPrefs.GetInt("LastScore", 0);
        float time = PlayerPrefs.GetFloat("LastTime", 0f);

        if (scoreText != null) scoreText.text = "Score: " + score;
        if (timeText != null) timeText.text = "Time: " + GameManager.FormatTime(time);
    }

    public void btnRetry()
    {


        if (GameManager.Instance != null && GameManager.Instance.hasWon)
            GameManager.Instance.Continuar();

        SceneManager.LoadScene("Start");
    }

    public void btnHome()
    {

        if (GameManager.Instance != null)
            GameManager.Instance.NuevaPartida();

        SceneManager.LoadScene("MainMenu");
    }
}