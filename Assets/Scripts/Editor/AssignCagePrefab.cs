using UnityEngine;
using UnityEditor;

public class AssignCagePrefab
{
    public static void Assign()
    {
        string prefabPath = "Assets/Prefabs/CagePrefab.prefab";
        string configPath = "Assets/Resources/GameConfig.asset";

        GameObject cagePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (cagePrefab == null)
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

        config.cagePrefab = cagePrefab;
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        
        Debug.Log("Successfully assigned CagePrefab to GameConfig!");
    }
}
