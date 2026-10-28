using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject coinPrefab;
    public Transform player;
    public float maxX = 2.5f;
    public float minGap = 1.2f;
    public float maxGap = 2.5f;
    public float spawnAhead = 12f;

    [Range(0f, 1f)]
    public float coinChance = 0.02f;
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

        if (Random.value < coinChance)
        {
            Vector3 coinPos = platform.transform.position + new Vector3(0f, coinOffsetY, 0f);
            Instantiate(coinPrefab, coinPos, Quaternion.identity);
        }

        nextY += Random.Range(minGap, maxGap);
    }
}