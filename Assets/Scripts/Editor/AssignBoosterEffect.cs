using UnityEngine;
using UnityEditor;

public class AssignBoosterEffect
{
    public static void Assign()
    {
        string prefabPath = "Assets/Polygon Arsenal/Prefabs/Interactive/Treasure/Explosion/Star/StarExplosionGold.prefab";
        string configPath = "Assets/Resources/GameConfig.asset";

        GameObject effectPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (effectPrefab == null)
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

        config.boosterLandEffectPrefab = effectPrefab;
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        
        Debug.Log("Successfully assigned StarExplosionGold prefab to GameConfig!");
    }
}
