using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    [SerializeField] Enemy[] enemyPrefabs;

    [Tooltip("Will select a random point from the provided points.")]
    [SerializeField] Transform[] spawnPoints;
    
    [Header("Spawning Mechanics"), Tooltip("Sets delay between waves of spawning.")]
    [SerializeField] float respawnRate = 10f;
    [Tooltip("How long until the first enemy spawns.")]
    [SerializeField] float initialSpawnDelay;
    [Tooltip("How many total enemies spawn.")]
    [SerializeField] int totalNumberToSpawn;
    [Tooltip("How many enemies spawn each spawn.")]
    [SerializeField] int numberToSpawnEachTime = 1;
    [Tooltip("If spawning should be enabled for this spawner.")]
    [SerializeField] bool spawnEnabled = false;

    float spawnTimer;
    int totalNumberSpawned;


    void OnEnable()
    {
        spawnTimer = respawnRate - initialSpawnDelay;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (ShouldSpawn())
            Spawn();
    }


    bool ShouldSpawn()
    {
        if (spawnEnabled == false)
            return false;
        if (totalNumberToSpawn > 0 && totalNumberSpawned >= totalNumberToSpawn)
            return false;

        return spawnTimer >= respawnRate;
    }

    void Spawn()
    {
        spawnTimer = 0;
        var availableSpawnPoints = spawnPoints.ToList();

        for (int i = 0; i < numberToSpawnEachTime; i++)
        {
            if (totalNumberToSpawn > 0 && totalNumberSpawned >= totalNumberToSpawn)
                break;

            Enemy prefab = ChooseRandomEnemyPrefab();
            if (prefab != null)
            {
                Transform spawnPoint = ChooseRandomSpawnPoint(availableSpawnPoints);

                if (availableSpawnPoints.Contains(spawnPoint))
                    availableSpawnPoints.Remove(spawnPoint);

                // var enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

                var enemy = prefab.Get<Enemy>(spawnPoint.position, spawnPoint.rotation);
                

                totalNumberSpawned++;
            }
        }
    }

    Transform ChooseRandomSpawnPoint(List<Transform> availableSpawnPoints)
    {
        if (availableSpawnPoints.Count == 0)
            return transform;

        if (availableSpawnPoints.Count == 1)
            return availableSpawnPoints[0];

        int index = UnityEngine.Random.Range(0, availableSpawnPoints.Count);

        return availableSpawnPoints[index];
    }

    Enemy ChooseRandomEnemyPrefab()
    {
        if (enemyPrefabs.Length == 0)
            return null;

        if (enemyPrefabs.Length == 1)
            return enemyPrefabs[0];

        int index = UnityEngine.Random.Range(0, enemyPrefabs.Length);

        return enemyPrefabs[index];
    }

#if UNITY_EDITOR

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one);

        foreach (var spawnPoint in spawnPoints)
        {
            Gizmos.DrawSphere(spawnPoint.position, 0.5f);
            Debug.DrawLine(spawnPoint.position, transform.position, Color.red);
        }
    }


#endif
}