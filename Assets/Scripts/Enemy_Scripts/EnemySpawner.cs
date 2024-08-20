using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public GameObject[] enemyPrefabs; // Array of enemy prefabs
        public int[] enemyCounts;         // Corresponding counts for each prefab
        public float spawnStartTime;      // Time when the wave starts
        public float minSpawnInterval;    // Minimum time interval between enemy spawns
        public float maxSpawnInterval;    // Maximum time interval between enemy spawns
        public Vector2 spawnDistanceRange; // Min and max distance from the spawner
    }

    public Wave[] waves;                      // Array of waves
    private List<GameObject> activeEnemies = new List<GameObject>(); // List to keep track of active enemies
    private List<Coroutine> activeWaveCoroutines = new List<Coroutine>(); // List to track active wave coroutines

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            int waveIndex = i;
            Wave wave = waves[waveIndex];

            yield return new WaitForSeconds(wave.spawnStartTime);

            Coroutine waveCoroutine = StartCoroutine(SpawnWave(waveIndex));
            activeWaveCoroutines.Add(waveCoroutine);
        }
    }

    IEnumerator SpawnWave(int waveIndex)
    {
        Wave wave = waves[waveIndex];

        while (true)
        {
            for (int i = 0; i < wave.enemyPrefabs.Length; i++)
            {
                for (int j = 0; j < wave.enemyCounts[i]; j++)
                {
                    float spawnDistance = Random.Range(wave.spawnDistanceRange.x, wave.spawnDistanceRange.y);
                    float angle = Random.Range(0f, Mathf.PI * 2);
                    Vector2 spawnPosition = (Vector2)transform.position + new Vector2(
                        Mathf.Cos(angle) * spawnDistance,
                        Mathf.Sin(angle) * spawnDistance
                    );

                    GameObject enemy = Instantiate(wave.enemyPrefabs[i], spawnPosition, Quaternion.identity);
                    activeEnemies.Add(enemy);
                    enemy.AddComponent<EnemyTracker>().OnDestroyed += () => activeEnemies.Remove(enemy);

                    float spawnInterval = Random.Range(wave.minSpawnInterval, wave.maxSpawnInterval);
                    yield return new WaitForSeconds(spawnInterval);
                }
            }

            yield return new WaitUntil(() => activeEnemies.Count == 0);

            // If it's the last wave or the next wave hasn't started yet, respawn this wave
            if (waveIndex == waves.Length - 1 || Time.time < waves[waveIndex + 1].spawnStartTime)
            {
                continue; 
            }
            else
            {
                break; 
            }
        }
    }
}
