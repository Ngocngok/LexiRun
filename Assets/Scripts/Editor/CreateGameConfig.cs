using UnityEngine;
using UnityEditor;

public class CreateGameConfig
{
    [MenuItem("LexiRun/Create Game Config")]
    public static void CreateConfig()
    {
        GameConfig config = ScriptableObject.CreateInstance<GameConfig>();
        
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        
        AssetDatabase.CreateAsset(config, "Assets/Resources/GameConfig.asset");
        AssetDatabase.SaveAssets();
        
        Debug.Log("GameConfig created at Assets/Resources/GameConfig.asset");
    }
}
