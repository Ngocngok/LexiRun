using UnityEngine;
using UnityEditor;

public class AssignSpeedTrail
{
    public static void Assign()
    {
        string prefabPath = "Assets/Polygon Arsenal/Prefabs/Interactive/Trails/AirTrails/FireTrail.prefab";
        string configPath = "Assets/Resources/GameConfig.asset";

        GameObject trailPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (trailPrefab == null)
        {
            Debug.LogError($"Could not find prefab at {prefabPath}");
            return;
        }

        GameConfig config = AssetDatabase.LoadAssetAtPath<GameConfig>(configPath);
        if (config == null)
        {
            Debug.LogError($"Could not find GameConfig at {configPath}");
            return;
        }

        config.speedUpTrailPrefab = trailPrefab;
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        
        Debug.Log("Successfully assigned FireTrail prefab to GameConfig!");
    }
}
