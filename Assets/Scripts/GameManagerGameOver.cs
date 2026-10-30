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
        SceneManager.LoadScene("MainMenu");
    }
   
}
