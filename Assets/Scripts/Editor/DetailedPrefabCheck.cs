using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Text;

public class DetailedPrefabCheck : MonoBehaviour
{
    public static string Execute()
    {
        string prefabPath = "Assets/Prefabs/BotInfoUI.prefab";
        
        // Force reload the asset
        AssetDatabase.Refresh();
        
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        
        if (prefab == null)
        {
            return "Prefab not found at: " + prefabPath;
        }
        
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Detailed BotInfoUI Prefab Check:");
        sb.AppendLine("================================");
        sb.AppendLine("Child count: " + prefab.transform.childCount);
        
        for (int i = 0; i < prefab.transform.childCount; i++)
        {
            Transform child = prefab.transform.GetChild(i);
            sb.AppendLine($"Child {i}: {child.name}");
        }
        
        sb.AppendLine("\nChecking specific children:");
        sb.AppendLine("BotNameText: " + (prefab.transform.Find("BotNameText") != null ? "Found" : "Not found"));
        sb.AppendLine("BotWordText: " + (prefab.transform.Find("BotWordText") != null ? "Found" : "Not found"));
        sb.AppendLine("BotMistakesText: " + (prefab.transform.Find("BotMistakesText") != null ? "Found" : "Not found"));
        sb.AppendLine("BotWordsCompletedText: " + (prefab.transform.Find("BotWordsCompletedText") != null ? "Found" : "Not found"));
        sb.AppendLine("BotCurrentWordProgressText: " + (prefab.transform.Find("BotCurrentWordProgressText") != null ? "Found" : "Not found"));
        
        // Check BotInfoUI component
        BotInfoUI botInfoUI = prefab.GetComponent<BotInfoUI>();
        if (botInfoUI != null)
        {
            sb.AppendLine("\nBotInfoUI component found!");
            sb.AppendLine("botNameText: " + (botInfoUI.botNameText != null ? "Assigned" : "NULL"));
            sb.AppendLine("botWordText: " + (botInfoUI.botWordText != null ? "Assigned" : "NULL"));
            sb.AppendLine("botMistakesText: " + (botInfoUI.botMistakesText != null ? "Assigned" : "NULL"));
            sb.AppendLine("botCurrentWordProgressText: " + (botInfoUI.botCurrentWordProgressText != null ? "Assigned" : "NULL"));
        }
        else
        {
            sb.AppendLine("\nBotInfoUI component NOT found!");
        }
        
        return sb.ToString();
    }
}
