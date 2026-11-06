using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerGameOver : MonoBehaviour
{
    
    public void btnRetry()
    {
        SceneManager.LoadScene("Start");
    }

    public void btnHome()
    {
        // Volver al menú = partida nueva, así que se borra el checkpoint
        if (GameManager.Instance != null)
            GameManager.Instance.ResetCheckpoint();

        SceneManager.LoadScene("MainMenu");
    }
   
}