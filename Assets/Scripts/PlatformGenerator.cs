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
    public float coinChance = 0.02f;
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
}