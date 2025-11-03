using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SetupBotPrefabs
{
    [MenuItem("LexiRun/Setup Bot Prefabs")]
    public static void Setup()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        if (gameManager != null)
        {
            GameObject[] botPrefabs = new GameObject[3];
            botPrefabs[0] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Bot_Cow.prefab");
            botPrefabs[1] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Bot_Pig.prefab");
            botPrefabs[2] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Bot_Buffalo.prefab");
            
            SerializedObject so = new SerializedObject(gameManager);
            SerializedProperty botPrefabsProp = so.FindProperty("botPrefabs");
            
            botPrefabsProp.arraySize = 3;
            botPrefabsProp.GetArrayElementAtIndex(0).objectReferenceValue = botPrefabs[0];
            botPrefabsProp.GetArrayElementAtIndex(1).objectReferenceValue = botPrefabs[1];
            botPrefabsProp.GetArrayElementAtIndex(2).objectReferenceValue = botPrefabs[2];
            
            so.ApplyModifiedProperties();
            
            EditorUtility.SetDirty(gameManager);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            
            Debug.Log("Bot prefabs setup complete!");
        }
    }
}
