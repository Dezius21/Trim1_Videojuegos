using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerPoints = 0;
    public float currentTime = 0f;
    public bool isTimerRunning = true;

    public int highScore = 0;
    public float bestTime = 0f;

    public bool hasWon = false;
    public bool modoInfinito = false;
    public Vector2 victoryPos;


    public bool isMultiplierActive = false;

    public bool hasCheckpoint = false;
    public Vector2 checkpointPos;
    public int checkpointScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            highScore = PlayerPrefs.GetInt("HighScore", 0);
            bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
        }
    }

    public void ActivarMultiplicador(float duracion)
    {
        StartCoroutine(RutinaMultiplicador(duracion));
    }

    private System.Collections.IEnumerator RutinaMultiplicador(float duracion)
    {
        isMultiplierActive = true;
        yield return new WaitForSeconds(duracion);
        isMultiplierActive = false;
    }

    public void IncreasePlayerPointsCoins()
    {
        playerPoints += isMultiplierActive ? 300 : 150;
    }

    public void SumarPuntosPorDistancia(int puntos)
    {
        playerPoints += isMultiplierActive ? (puntos * 2) : puntos;
    }



    public void NuevaPartida()
    {
        hasCheckpoint = false;
        checkpointScore = 0;
        playerPoints = 0;
        currentTime = 0f;
        hasWon = false;
        modoInfinito = false;
        isTimerRunning = true;
        Time.timeScale = 1f;
    }



    public void Continuar()
    {
        modoInfinito = true;
        hasWon = false;
        SetCheckpoint(victoryPos, playerPoints);
        isTimerRunning = true;
        Time.timeScale = 1f;
    }


    public void Victoria(Vector2 pos)
    {

        if (hasWon || modoInfinito) return;
        hasWon = true;
        victoryPos = pos;
        isTimerRunning = false;


        if (bestTime <= 0f || currentTime < bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        GuardarDatosPartida();
    }

    public void SetCheckpoint(Vector2 pos, int score)
    {
        hasCheckpoint = true;
        checkpointPos = pos;
        checkpointScore = score;
    }

    public void ResetCheckpoint()
    {
        hasCheckpoint = false;
        checkpointScore = 0;
    }



    public void ResetScore()
    {
        Time.timeScale = 1f;


        playerPoints = hasCheckpoint ? checkpointScore : 0;

        isTimerRunning = true;
    }

    public void GuardarDatosPartida()
    {
        if (playerPoints > highScore)
        {
            highScore = playerPoints;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        PlayerPrefs.SetInt("LastScore", playerPoints);
        PlayerPrefs.SetFloat("LastTime", currentTime);
        PlayerPrefs.Save();
    }


    public static string FormatTime(float t)
    {
        int minutos = Mathf.FloorToInt(t / 60f);
        int segundos = Mathf.FloorToInt(t % 60f);
        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}