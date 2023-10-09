using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    public float spawnInterval = 2f;
    public float spawnIntervalDecrement = 0.05f;
    public float minSpawnInterval = 0.5f;

    private float nextSpawnTime;

    private void Start(){
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update(){
        if (Time.time >= nextSpawnTime){
            // Create a list to hold the out-of-view spawn points
            List<Transform> outOfViewSpawnPoints = new List<Transform>();

            foreach (Transform spawnPoint in spawnPoints){
                if (!IsSpawnPointVisible(spawnPoint)){
                    outOfViewSpawnPoints.Add(spawnPoint);
                }
            }

            if (outOfViewSpawnPoints.Count > 0){
                Transform randomSpawnPoint = outOfViewSpawnPoints[Random.Range(0, outOfViewSpawnPoints.Count)];
                SpawnEnemy(randomSpawnPoint);
                CalculateNextSpawnTime();
            }
        }
    }

    private void SpawnEnemy(Transform spawnPoint){
        if (enemyPrefabs.Length == 0){
            Debug.LogWarning("No enemy prefabs assigned to the enemy spawner!");
            return;
        }

        GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(randomEnemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void CalculateNextSpawnTime(){
        spawnInterval -= spawnIntervalDecrement;
        spawnInterval = Mathf.Max(spawnInterval, minSpawnInterval);
        nextSpawnTime = Time.time + spawnInterval;
    }

    private bool IsSpawnPointVisible(Transform spawnPoint){
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(spawnPoint.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1 && screenPoint.z > 0;
    }
}
