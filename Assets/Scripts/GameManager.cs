using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerPoints = 0;
    public int highScore = 0;

    //Estado del multiplicador
    public bool isMultiplierActive = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
    }

    // --- CHECKPOINT ---
    public bool hasCheckpoint = false;
    public Vector2 checkpointPos;
    public int checkpointScore;

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
        if (playerPoints > highScore) highScore = playerPoints;
        // Si hay checkpoint, la partida continua con los puntos que tenías al activarlo
        playerPoints = hasCheckpoint ? checkpointScore : 0;
    }


}
