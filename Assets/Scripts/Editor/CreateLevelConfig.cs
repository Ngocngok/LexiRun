using UnityEngine;
using UnityEditor;

public class CreateLevelConfig
{
    [MenuItem("LexiRun/Create Level Config")]
    public static void CreateConfig()
    {
        LevelConfig config = ScriptableObject.CreateInstance<LevelConfig>();
        
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        
        AssetDatabase.CreateAsset(config, "Assets/Resources/LevelConfig.asset");
        AssetDatabase.SaveAssets();
        
        Debug.Log("LevelConfig created at Assets/Resources/LevelConfig.asset");
    }
}
