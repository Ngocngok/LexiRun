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
        GameObject boosterObj = new GameObject("Booster");
        boosterObj.transform.SetParent(transform);

        Booster booster = boosterObj.AddComponent<Booster>();
        
        // Random type
        BoosterType randomType = (BoosterType)Random.Range(0, System.Enum.GetValues(typeof(BoosterType)).Length);
        booster.type = randomType;

        // Position randomly
        float halfWidth = config.arenaWidth / 2f - 1f;
        float halfHeight = config.arenaHeight / 2f - 1f;

        Vector3 randomPos = new Vector3(
            Random.Range(-halfWidth, halfWidth),
            0.5f,
            Random.Range(-halfHeight, halfHeight)
        );

        boosterObj.transform.position = randomPos;

        activeBoosters.Add(boosterObj);
    }
}
