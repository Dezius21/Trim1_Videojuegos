using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void btnPlay()
    {
        SceneManager.LoadScene("Start");

    }

    public void btnQuit()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
