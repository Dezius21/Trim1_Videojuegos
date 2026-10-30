using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject coinPrefab;
    public GameObject powerUpPrefab; // Nuevo
    public GameObject enemyPrefab;   // Nuevo
    
    public Transform player;
    public float maxX = 2.5f;
    public float minGap = 1.2f;
    public float maxGap = 2.5f;
    public float spawnAhead = 12f;

    [Range(0f, 1f)]
    public float coinChance = 0.2f;
    [Range(0f, 1f)]
    public float powerUpChance = 0.03f; // 3% de probabilidad
    [Range(0f, 1f)]
    public float enemyChance = 0.10f;   // 10% de probabilidad

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
        GameObject platform = Instantiate(platformPrefab, new Vector3(x, nextY, 0), Quaternion.identity);

        // Tiramos un único "dado" virtual del 0.0 al 1.0
        float tirada = Random.value;

        if (tirada < coinChance)
        {
            Vector3 coinPos = platform.transform.position + new Vector3(0f, coinOffsetY, 0f);
            Instantiate(coinPrefab, coinPos, Quaternion.identity);
        }
        else if (tirada < coinChance + powerUpChance)
        {
            Vector3 powerUpPos = platform.transform.position + new Vector3(0f, coinOffsetY, 0f);
            Instantiate(powerUpPrefab, powerUpPos, Quaternion.identity);
        }
        else if (tirada < coinChance + powerUpChance + enemyChance)
        {
            // El enemigo aparece en la misma Y, pero en una X aleatoria para obligar a esquivarlo
            float enemyX = Random.Range(-maxX, maxX);
            Vector3 enemyPos = new Vector3(enemyX, nextY + (minGap / 2), 0f);
            Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
        }

        nextY += Random.Range(minGap, maxGap);
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