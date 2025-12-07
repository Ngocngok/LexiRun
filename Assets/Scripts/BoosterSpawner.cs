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
        if (config.mysteryBoosterPrefab == null)
        {
            Debug.LogWarning("Mystery Booster Prefab not assigned in GameConfig!");
            return;
        }

        // Position randomly
        float halfWidth = config.arenaWidth / 2f - 3f;
        float halfHeight = config.arenaHeight / 2f - 3f;

        Vector3 randomPos = new Vector3(
            Random.Range(-halfWidth, halfWidth),
            10f, // Start high up to fall
            Random.Range(-halfHeight, halfHeight)
        );

        GameObject boosterObj = Instantiate(config.mysteryBoosterPrefab, randomPos, Quaternion.identity, transform);
        activeBoosters.Add(boosterObj);
    }
}
