using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AssignBotInfoUIReferences : MonoBehaviour
{
    public static string Execute()
    {
        string prefabPath = "Assets/Prefabs/BotInfoUI.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        
        if (prefab == null)
        {
            return "Prefab not found at: " + prefabPath;
        }
        
        // Get the BotInfoUI component
        BotInfoUI botInfoUI = prefab.GetComponent<BotInfoUI>();
        if (botInfoUI == null)
        {
            return "BotInfoUI component not found on prefab.";
        }
        
        // Find and assign all text references
        Transform botNameText = prefab.transform.Find("BotNameText");
        Transform botWordText = prefab.transform.Find("BotWordText");
        Transform botMistakesText = prefab.transform.Find("BotMistakesText");
        Transform botWordsCompletedText = prefab.transform.Find("BotWordsCompletedText");
        Transform botCurrentWordProgressText = prefab.transform.Find("BotCurrentWordProgressText");
        
        // Use SerializedObject to modify the prefab
        SerializedObject serializedObject = new SerializedObject(botInfoUI);
        
        if (botNameText != null)
        {
            serializedObject.FindProperty("botNameText").objectReferenceValue = botNameText.GetComponent<Text>();
        }
        
        if (botWordText != null)
        {
            serializedObject.FindProperty("botWordText").objectReferenceValue = botWordText.GetComponent<Text>();
        }
        
        if (botMistakesText != null)
        {
            serializedObject.FindProperty("botMistakesText").objectReferenceValue = botMistakesText.GetComponent<Text>();
        }
        
        if (botWordsCompletedText != null)
        {
            serializedObject.FindProperty("botWordsCompletedText").objectReferenceValue = botWordsCompletedText.GetComponent<Text>();
        }
        
        if (botCurrentWordProgressText != null)
        {
            serializedObject.FindProperty("botCurrentWordProgressText").objectReferenceValue = botCurrentWordProgressText.GetComponent<Text>();
        }
        
        serializedObject.ApplyModifiedProperties();
        
        // Save the prefab
        PrefabUtility.SavePrefabAsset(prefab);
        
        return "Successfully assigned all references in BotInfoUI prefab.";
    }
}
