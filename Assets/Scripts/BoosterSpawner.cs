using UnityEngine;
using System.Collections.Generic;

public class BoosterSpawner : MonoBehaviour
{
    private float spawnTimer;
    private List<GameObject> activeBoosters = new List<GameObject>();
    private GameConfig config;

    void Start()
    {
        config = GameManager.Instance.config;
        ResetTimer();
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameActive()) return;

        // Clean up null entries
        activeBoosters.RemoveAll(item => item == null);

        if (activeBoosters.Count >= config.maxBoosters) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnBooster();
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        spawnTimer = Random.Range(config.boosterSpawnIntervalMin, config.boosterSpawnIntervalMax);
    }

    private void SpawnBooster()
    {
        if (config.boosterPrefabs == null || config.boosterPrefabs.Length == 0)
        {
            Debug.LogWarning("Booster Prefabs not assigned in GameConfig!");
            return;
        }

        // Random type
        BoosterType randomType = (BoosterType)Random.Range(0, System.Enum.GetValues(typeof(BoosterType)).Length);
        
        // Find matching prefab
        GameObject prefabToSpawn = null;
        foreach (var prefab in config.boosterPrefabs)
        {
            if (prefab != null)
            {
                Booster b = prefab.GetComponent<Booster>();
                if (b != null && b.type == randomType)
                {
                    prefabToSpawn = prefab;
                    break;
                }
            }
        }

        if (prefabToSpawn == null)
        {
            // Fallback: just pick a random one if exact match fails (shouldn't happen if setup correctly)
             prefabToSpawn = config.boosterPrefabs[Random.Range(0, config.boosterPrefabs.Length)];
        }

        // Position randomly
        float halfWidth = config.arenaWidth / 2f - 1f;
        float halfHeight = config.arenaHeight / 2f - 1f;

        Vector3 randomPos = new Vector3(
            Random.Range(-halfWidth, halfWidth),
            0.5f,
            Random.Range(-halfHeight, halfHeight)
        );

        GameObject boosterObj = Instantiate(prefabToSpawn, randomPos, Quaternion.identity, transform);
        activeBoosters.Add(boosterObj);
    }
}
