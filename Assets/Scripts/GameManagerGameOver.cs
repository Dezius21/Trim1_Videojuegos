using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Lo usan las escenas GameOver y Victory
public class GameManagerGameOver : MonoBehaviour
{
    public TMP_Text scoreText;   // opcional (GameOver)
    public TMP_Text timeText;    // GameOver y Victory

    void Start()
    {
        int score = PlayerPrefs.GetInt("LastScore", 0);
        float time = PlayerPrefs.GetFloat("LastTime", 0f);

        if (scoreText != null) scoreText.text = "Score: " + score;
        if (timeText != null) timeText.text = "Time: " + GameManager.FormatTime(time);
    }

    public void btnRetry()
    {
        // Botón de la escena Victory (hasWon = true) = CONTINUAR: misma puntuación y tiempo, desde la meta.
        // Botón de la escena GameOver = Retry: se conserva el tiempo.
        if (GameManager.Instance != null && GameManager.Instance.hasWon)
            GameManager.Instance.Continuar();

        SceneManager.LoadScene("Start");
    }

    public void btnHome()
    {
        // Volver al menú = partida nueva (borra checkpoint y tiempo)
        if (GameManager.Instance != null)
            GameManager.Instance.NuevaPartida();

        SceneManager.LoadScene("MainMenu");
    }
}