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
    public int puntosPorCheckpoint = 1000;
    private float checkpointStep;
    private float nextCheckpointY;

    private float nextY = 0f;
    private float prevY = 0f;

    [Header("Meta / Victoria")]
    public GameObject metaPrefab;
    public int scoreVictoria = 10000;
    private bool metaGenerada = false;

    void Start()
    {

        if (GameManager.Instance != null)
            GameManager.Instance.ResetScore();


        checkpointStep = puntosPorCheckpoint / 10f;
        nextCheckpointY = checkpointStep;


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



        if (!metaGenerada && metaPrefab != null && GameManager.Instance != null
            && !GameManager.Instance.modoInfinito)
        {
            float puntosRestantes = scoreVictoria - GameManager.Instance.playerPoints;
            float yMeta = player.position.y + puntosRestantes / 10f;


            if (yMeta <= nextY)
            {

                yMeta = Mathf.Min(nextY, Mathf.Max(yMeta, prevY + minGap));

                Instantiate(metaPrefab, new Vector3(0f, yMeta, 0f), Quaternion.identity);
                metaGenerada = true;
                prevY = yMeta;
                nextY = yMeta + Random.Range(minGap, maxGap);
                return;
            }
        }


        if (checkpointPrefab != null && !metaGenerada && nextY >= nextCheckpointY)
        {
            float cx = Random.Range(-maxX, maxX);
            Instantiate(checkpointPrefab, new Vector3(cx, nextY, 0), Quaternion.identity);
            nextCheckpointY += checkpointStep;
            Avanzar(Random.Range(minGap, maxGap));
            return;
        }

        float x = Random.Range(-maxX, maxX);
        GameObject platform;


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
            Avanzar(baseGap + rangoMovimientoVertical);
        }
        else
        {
            Avanzar(baseGap);
        }

    }


    void Avanzar(float gap)
    {
        prevY = nextY;
        nextY += gap;
    }


    void AjustarDificultad()
    {
        if (GameManager.Instance == null) return;

        int puntosActuales = GameManager.Instance.playerPoints;

        if (puntosActuales <= 2500)
        {
            minGap = 1.2f;
            maxGap = 2.5f;
            enemyChance = 0.03f;
            horizontalPlatformChance = 0f;
            verticalPlatformChance = 0f;
        }
        else if (puntosActuales > 2500 && puntosActuales <= 5000)
        {
            minGap = 1.4f; 
            maxGap = 2.65f;
            enemyChance = 0.07f;
            horizontalPlatformChance = 0.1f;
            verticalPlatformChance = 0.05f;
        }
        else if (puntosActuales > 5000 && puntosActuales <= 7500)
        {
            minGap = 1.6f; 
            maxGap = 2.75f;
            enemyChance = 0.1f;
            horizontalPlatformChance = 0.15f;
            verticalPlatformChance = 0.1f;
            puntosPorCheckpoint = 1200;
        }
        else if (puntosActuales > 7500)
        {
            minGap = 1.8f; 
            maxGap = 2.9f;
            enemyChance = 0.2f;
            horizontalPlatformChance = 0.2f;
            verticalPlatformChance = 0.15f;
            puntosPorCheckpoint = 0;
        }
    }
}