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
        float halfWidth = config.arenaWidth / 2f - 3f; // Padding
        float halfHeight = config.arenaHeight / 2f - 3f;

        Vector3 targetPos = new Vector3(
            Random.Range(-halfWidth, halfWidth),
            0.5f, // Height
            Random.Range(-halfHeight, halfHeight)
        );

        // Start Position (Outside map)
        // Pick a random direction
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Mathf.Max(config.arenaWidth, config.arenaHeight); // Start from edge distance
        Vector3 startPos = new Vector3(
            Mathf.Cos(angle) * distance,
            10f, // Start high
            Mathf.Sin(angle) * distance
        );

        GameObject bombObj = Instantiate(config.bombPrefab, startPos, Quaternion.identity, transform);
        activeBombs.Add(bombObj);

        Bomb bombScript = bombObj.GetComponent<Bomb>();
        if (bombScript != null)
        {
            bombScript.Initialize(startPos, targetPos, 2.5f); // 2.5 seconds for 3 bounces
        }
    }
}
