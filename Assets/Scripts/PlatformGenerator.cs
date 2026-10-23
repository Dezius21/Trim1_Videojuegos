using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public Transform player;
    public float maxX = 2.5f;
    public float minGap = 1.2f;
    public float maxGap = 2.5f;
    public float spawnAhead = 12f;

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
        Instantiate(platformPrefab, new Vector3(x, nextY, 0), Quaternion.identity);
        nextY += Random.Range(minGap, maxGap);
    }
}