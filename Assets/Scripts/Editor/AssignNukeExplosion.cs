using UnityEngine;
using UnityEditor;

public class AssignNukeExplosion
{
    public static void Assign()
    {
        string prefabPath = "Assets/Polygon Arsenal/Prefabs/Combat/Explosions/Sci-Fi/Nuke/NukeExplosionRed.prefab";
        string configPath = "Assets/Resources/GameConfig.asset";

        GameObject nukePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (nukePrefab == null)
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

        config.nukeExplosionVFX = nukePrefab;
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        
        Debug.Log("Successfully assigned NukeExplosionRed prefab to GameConfig!");
    }
}
