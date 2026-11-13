using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerPoints = 0;
    public float currentTime = 0f;
    public bool isTimerRunning = true;

    public int highScore = 0;
    public float bestTime= 0f;

    public int winScoreTarget = 10000;
    public bool hasWon = false;

    //Estado del multiplicador
    public bool isMultiplierActive = false;


    public bool hasCheckpoint = false;
    public Vector2 checkpointPos;
    public int checkpointScore;
    public float checkpointTime;

    private void Awake()
    {
        if(Instance == null)
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
        if(isTimerRunning)
        {
            currentTime += Time.deltaTime;
        }
    }

    public void ActivarMultiplicador (float duracion)
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
        playerPoints += isMultiplierActive ? 300: 150;
        ComprobarVictoria();
    }
/*
    public void IncreasePlayerPointsHeigth()
    {
        playerPoints += 10;
    }
    */

    public void SumarPuntosPorDistancia(int puntos)
    {
        playerPoints += isMultiplierActive ? (puntos * 2) : puntos;
        ComprobarVictoria();
    }

    private void ComprobarVictoria()
    {
        if (!hasWon && playerPoints >= winScoreTarget)
        {
            hasWon = true;
            isTimerRunning = false;
            GuardarDatosPartida();

            Time.timeScale = 0f;

            Debug.Log("¡Victoria Alcanzada! El juego está pausado");
        }
    }

    public void ContinuarModoInfinito()
    {
        Time.timeScale = 1f;
        isTimerRunning = true;
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
        if (playerPoints > highScore)
        {
            highScore = playerPoints;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        if (currentTime > bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        Time.timeScale = 1f;

        // Si hay checkpoint, la partida continua con los puntos que tenías al activarlo
        playerPoints = hasCheckpoint ? checkpointScore : 0;
        currentTime = hasCheckpoint ? checkpointTime :  0f;

        isTimerRunning = true;
    }

    public void GuardarDatosPartida()
    {
        PlayerPrefs.SetInt("LastScore", playerPoints);
        PlayerPrefs.SetFloat("LastTime", currentTime);
        PlayerPrefs.Save();
    }

}
