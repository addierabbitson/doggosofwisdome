using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public List<Enemy> enemyPrefabs;
    [Tooltip("Where enemies will move towards")]
    public Vector3 endPoint = Vector3.zero;
    [Tooltip("How many seconds between each spawn")]
    public float spawnInterval = 1.0f;
    [Tooltip("Maximum variance of the spawn interval (in seconds)")]
    public float spawnVariance = 1.0f;

    float spawnTimer = 0.0f;

    private void Awake()
    {
        spawnTimer += Random.Range(0.0f, spawnInterval);
    }

    private void FixedUpdate()
    {
        spawnTimer -= Time.fixedDeltaTime;

        if (spawnTimer > 0)
            return;

        spawnTimer = spawnInterval + Random.Range(0.0f, spawnVariance);

        Enemy toSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        Enemy newEnemy = Instantiate(toSpawn, transform);
        newEnemy.Spawn(transform.position, endPoint);
    }

}
