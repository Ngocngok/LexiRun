using UnityEngine;
using System.Collections.Generic;

public class BombSpawner : MonoBehaviour
{
    private float spawnTimer;
    private List<GameObject> activeBombs = new List<GameObject>();
    private GameConfig config;

    void Start()
    {
        config = GameManager.Instance.config;
        ResetTimer();
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameActive()) return;

        // Clean up null entries in the list (destroyed bombs)
        activeBombs.RemoveAll(item => item == null);

        if (activeBombs.Count >= config.maxBombs) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnBomb();
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        spawnTimer = Random.Range(config.bombSpawnIntervalMin, config.bombSpawnIntervalMax);
    }

    private void SpawnBomb()
    {
        if (config.bombPrefab == null)
        {
            Debug.LogWarning("Bomb Prefab not assigned in GameConfig!");
            return;
        }

        // Position randomly within arena
        // Arena is centered at 0,0
        float halfWidth = config.arenaWidth / 2f - 1f; // Padding
        float halfHeight = config.arenaHeight / 2f - 1f;

        Vector3 randomPos = new Vector3(
            Random.Range(-halfWidth, halfWidth),
            0.5f, // Height
            Random.Range(-halfHeight, halfHeight)
        );

        GameObject bombObj = Instantiate(config.bombPrefab, randomPos, Quaternion.identity, transform);
        activeBombs.Add(bombObj);
    }
}
