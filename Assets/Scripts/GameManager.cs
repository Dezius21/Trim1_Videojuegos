using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerPoints = 0;
    public float currentTime = 0f;
    public bool isTimerRunning = true;

    public int highScore = 0;
    public float bestTime = 0f;   // 0 = todavía no hay mejor tiempo

    public bool hasWon = false;
    public bool modoInfinito = false;   // true = ya has ganado y has pulsado Continuar
    public Vector2 victoryPos;          // posición de la plataforma de victoria

    //Estado del multiplicador
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

    // Se llama al empezar una partida DESDE CERO (botón Play, Home, o Retry tras ganar).
    // Es el ÚNICO sitio donde el tiempo vuelve a 0.
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

    // Botón Continuar de la escena Victory: sigues desde la plataforma de victoria,
    // con la misma puntuación y con el tiempo siguiendo desde donde estaba.
    public void Continuar()
    {
        modoInfinito = true;
        hasWon = false;
        SetCheckpoint(victoryPos, playerPoints);   // reaparece (y reintenta) desde la meta
        isTimerRunning = true;
        Time.timeScale = 1f;
    }

    // Se llama al tocar la plataforma de victoria
    public void Victoria(Vector2 pos)
    {
        // En modo continuar no se vuelve a ganar ni se toca el mejor tiempo
        if (hasWon || modoInfinito) return;
        hasWon = true;
        victoryPos = pos;
        isTimerRunning = false;

        // Mejor tiempo = el MENOR tiempo con el que se ha ganado
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

    // Se llama al cargar el nivel (también tras un Retry).
    // NO toca currentTime: el tiempo se conserva entre intentos.
    public void ResetScore()
    {
        Time.timeScale = 1f;

        // Si hay checkpoint, la partida continúa con los puntos que tenías al activarlo
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

    // Formato mm:ss para usar en cualquier pantalla
    public static string FormatTime(float t)
    {
        int minutos = Mathf.FloorToInt(t / 60f);
        int segundos = Mathf.FloorToInt(t % 60f);
        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}