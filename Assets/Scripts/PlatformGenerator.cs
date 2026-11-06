using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject coinPrefab;
    public GameObject powerUpPrefab; 
    public GameObject enemyPrefab;   
    public GameObject horizontalPlatformPrefab;
    public GameObject verticalPlatformPrefab;
    public GameObject checkpointPrefab;
    
    public Transform player;
    public float maxX = 2.5f;
    public float minGap = 1.2f;
    public float maxGap = 2.5f;
    public float spawnAhead = 12f;

    [Range(0f, 1f)]
    public float coinChance = 0.07f;
    [Range(0f, 1f)]
    public float powerUpChance = 0.05f;
    [Range(0f, 1f)]
    public float enemyChance = 0.03f;   
    [Range(0f, 1f)]
    public float horizontalPlatformChance = 0f; 
    [Range(0f, 1f)]
    public float verticalPlatformChance = 0f;   

    public float coinOffsetY = 0.5f;

    [Header("Checkpoint")]
    public int puntosPorCheckpoint = 300; // cada cuántos puntos de altura sale un checkpoint
    private float checkpointStep;
    private float nextCheckpointY;

    private float nextY = 0f;

    void Start()
    {
        // La puntuación por altura es Y * 10, así que X puntos = X / 10 unidades de altura
        checkpointStep = puntosPorCheckpoint / 10f;
        nextCheckpointY = checkpointStep;

        // Si venimos de morir con un checkpoint activo, empezamos el nivel desde él
        if (GameManager.Instance != null && GameManager.Instance.hasCheckpoint)
        {
            Vector2 cp = GameManager.Instance.checkpointPos;
            Instantiate(checkpointPrefab, new Vector3(cp.x, cp.y, 0f), Quaternion.identity);
            nextY = cp.y + Random.Range(minGap, maxGap);
            nextCheckpointY = cp.y + checkpointStep;
        }
        
        while (nextY < spawnAhead)
            SpawnPlatform();
    }

    void Update()
    {
        AjustarDificultad();
        while (nextY < player.position.y + spawnAhead)
            SpawnPlatform();
    }

    void SpawnPlatform()
    {
        // CHECKPOINT: cada X puntos sale una plataforma especial
        if (checkpointPrefab != null && nextY >= nextCheckpointY)
        {
            float cx = Random.Range(-maxX, maxX);
            Instantiate(checkpointPrefab, new Vector3(cx, nextY, 0), Quaternion.identity);
            nextCheckpointY += checkpointStep;
            nextY += Random.Range(minGap, maxGap);
            return;
        }

        float x = Random.Range(-maxX, maxX);
        GameObject platform;

        // 1. Decidir QUÉ TIPO de plataforma spawnear
        float tiradaPlataforma = Random.value;
        bool specialPlatform = false;
        bool isVertical = false;

        if (tiradaPlataforma < horizontalPlatformChance) 
        {
            platform = Instantiate(horizontalPlatformPrefab, new Vector3(x, nextY, 0), Quaternion.identity);
            specialPlatform = true;
        } 
        else if (tiradaPlataforma < horizontalPlatformChance + verticalPlatformChance) 
        {
            platform = Instantiate(verticalPlatformPrefab, new Vector3(x, nextY, 0), Quaternion.identity);
            specialPlatform = true;
            isVertical = true;
        } 
        else 
        {
            platform = Instantiate(platformPrefab, new Vector3(x, nextY, 0), Quaternion.identity);
        }

        // 2. Decidir QUÉ ÍTEM o ENEMIGO spawnear sobre/cerca de la plataforma
        if(!specialPlatform){
            float tiradaItem = Random.value;

            if (tiradaItem < coinChance) 
            {
                Vector3 coinPos = platform.transform.position + new Vector3(0f, coinOffsetY, 0f);
                Instantiate(coinPrefab, coinPos, Quaternion.identity);
            } 
            else if (tiradaItem < coinChance + powerUpChance) 
            {
                Vector3 powerUpPos = platform.transform.position + new Vector3(0f, coinOffsetY, 0f);
                Instantiate(powerUpPrefab, powerUpPos, Quaternion.identity);
            } 
            else if (tiradaItem < coinChance + powerUpChance + enemyChance) 
            {
                float enemyX = Random.Range(-maxX, maxX);
                Vector3 enemyPos = new Vector3(enemyX, nextY + (minGap / 2), 0f);
                Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
            }
        }

        float baseGap = Random.Range(minGap, maxGap);

        if (isVertical)
        {
            float rangoMovimientoVertical = 1.5f; 
            nextY += baseGap + rangoMovimientoVertical;
        }
        else
        {
            nextY += baseGap;
        }

    }


    void AjustarDificultad()
    {
        if (GameManager.Instance == null) return;

        int puntosActuales = GameManager.Instance.playerPoints;

        if (puntosActuales <= 500)
        {
            minGap = 1.2f;
            maxGap = 2.5f;
            enemyChance = 0.03f;
            horizontalPlatformChance = 0f;
            verticalPlatformChance = 0f;
        }
        else if (puntosActuales > 500 && puntosActuales <= 1000)
        {
            minGap = 1.4f; 
            maxGap = 2.65f;
            enemyChance = 0.07f;
            horizontalPlatformChance = 0.1f;
            verticalPlatformChance = 0.05f;
        }
        else if (puntosActuales > 1000 && puntosActuales <= 1500)
        {
            minGap = 1.6f; 
            maxGap = 2.75f;
            enemyChance = 0.1f;
            horizontalPlatformChance = 0.15f;
            verticalPlatformChance = 0.1f;
        }
        else if (puntosActuales > 1500)
        {
            minGap = 1.8f; 
            maxGap = 2.9f;
            enemyChance = 0.2f;
            horizontalPlatformChance = 0.2f;
            verticalPlatformChance = 0.15f;
        }
    }
}