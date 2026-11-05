using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject coinPrefab;
    public GameObject powerUpPrefab; 
    public GameObject enemyPrefab;   
    public GameObject horizontalPlatformPrefab;
    public GameObject verticalPlatformPrefab;
    
    public Transform player;
    public float maxX = 2.5f;
    public float minGap = 1.2f;
    public float maxGap = 2.5f;
    public float spawnAhead = 12f;

    [Range(0f, 1f)]
    public float coinChance = 0.2f;
    [Range(0f, 1f)]
    public float powerUpChance = 0.02f;
    [Range(0f, 1f)]
    public float enemyChance = 0.03f;   
    [Range(0f, 1f)]
    public float horizontalPlatformChance = 0.05f; 
    [Range(0f, 1f)]
    public float verticalPlatformChance = 0.05f;   

    public float coinOffsetY = 0.5f;

    private float nextY = 0f;

    void Start()
    {
        while (nextY < spawnAhead)
            SpawnPlatform();
    }

    void Update()
    {
        //AjustarDificultad();
        while (nextY < player.position.y + spawnAhead)
            SpawnPlatform();
    }

    void SpawnPlatform()
    {
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
/*
    void AjustarDificultad()
    {
        // Validamos que el GameManager exista para evitar errores en consola
        if (GameManager.Instance == null) return;

        int puntosActuales = GameManager.Instance.playerPoints;

        // SISTEMA DE DIFICULTAD ESCALABLE
        // Si muere, los puntos vuelven a 0, por lo que entra al primer "if" y minGap vuelve a 1.2f automáticamente.
        if (puntosActuales <= 50)
        {
            minGap = 1.2f;
            maxGap = 2.5f;
        }
        else if (puntosActuales > 50 && puntosActuales <= 200)
        {
            minGap = 3.0f; // Se empieza a poner más difícil
            maxGap = 4.0f;
        }
        else if (puntosActuales > 200)
        {
            minGap = 10.0f; // Dificultad máxima
            maxGap = 15.0f;
        }
    }
    */
}