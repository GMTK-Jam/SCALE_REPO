using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public GameObject enemyPrefab;
    public float spawnDistance = 10f;

    public float spawnInterval = 2f;
    private float spawnTimer;

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnDirection = Random.insideUnitCircle.normalized;

        Vector2 spawnPosition = (Vector2)player.position + spawnDirection * spawnDistance;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
