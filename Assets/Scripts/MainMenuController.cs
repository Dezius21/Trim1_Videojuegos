using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public TMP_Text bestTimeText;

    void Start()
    {
        float best = PlayerPrefs.GetFloat("BestTime", 0f);

        if (bestTimeText != null)
        {
            bestTimeText.text = best > 0f
                ? "Best time: " + GameManager.FormatTime(best)
                : "Best time: --:--";
        }
    }

    public void btnPlay()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.NuevaPartida();

        SceneManager.LoadScene("Start");
    }

    public void btnQuit()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}