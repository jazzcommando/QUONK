using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to be spawned
    public Transform[] spawnPoints; // Array of spawn points

    public float spawnInterval = 2f; // Time interval between enemy spawns
    public float spawnIntervalDecrement = 0.1f; // Decrease in spawn interval over time
    public float minSpawnInterval = 0.5f; // Minimum spawn interval (cap)

    private float nextSpawnTime; // Time to spawn the next enemy

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            CalculateNextSpawnTime();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs or spawn points assigned to the enemy spawner!");
            return;
        }

        // Select a random enemy prefab and spawn point
        GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn the enemy at the selected spawn point
        Instantiate(randomEnemyPrefab, randomSpawnPoint.position, Quaternion.identity);
    }

    private void CalculateNextSpawnTime()
    {
        spawnInterval -= spawnIntervalDecrement;
        spawnInterval = Mathf.Max(spawnInterval, minSpawnInterval);
        nextSpawnTime = Time.time + spawnInterval;
    }
}

