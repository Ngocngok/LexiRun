using UnityEngine;
using UnityEditor;

public class AssignStunPrefab
{
    public static void Assign()
    {
        string prefabPath = "Assets/Polygon Arsenal/Prefabs/Combat/Melee & Sword/Stun/Stun1.prefab";
        string configPath = "Assets/Resources/GameConfig.asset";

        GameObject stunPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (stunPrefab == null)
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

        config.stunEffectPrefab = stunPrefab;
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        
        Debug.Log("Successfully assigned Stun1 prefab to GameConfig!");
    }
}
