using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Text;

public class VerifyAndFixBotInfoUI : MonoBehaviour
{
    public static string Execute()
    {
        string prefabPath = "Assets/Prefabs/BotInfoUI.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        
        if (prefab == null)
        {
            return "Prefab not found at: " + prefabPath;
        }
        
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Checking BotInfoUI prefab...");
        
        // Check if BotCurrentWordProgressText exists
        Transform currentWordProgressText = prefab.transform.Find("BotCurrentWordProgressText");
        
        if (currentWordProgressText == null)
        {
            sb.AppendLine("BotCurrentWordProgressText not found. Creating it...");
            
            // Create a new GameObject for the current word progress text
            GameObject textObj = new GameObject("BotCurrentWordProgressText");
            textObj.transform.SetParent(prefab.transform, false);
            
            // Add RectTransform
            RectTransform rectTransform = textObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.sizeDelta = new Vector2(0, 30);
            rectTransform.anchoredPosition = Vector2.zero;
            
            // Add Text component
            Text text = textObj.AddComponent<Text>();
            text.text = "Current: 0/5";
            text.fontSize = 18;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleLeft;
            
            // Try to find and use the same font as other text elements
            Text referenceText = prefab.transform.Find("BotMistakesText")?.GetComponent<Text>();
            if (referenceText != null && referenceText.font != null)
            {
                text.font = referenceText.font;
                text.fontSize = referenceText.fontSize;
            }
            else
            {
                text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            }
            
            // Set sibling index to place it after BotMistakesText
            Transform mistakesText = prefab.transform.Find("BotMistakesText");
            if (mistakesText != null)
            {
                int mistakesIndex = mistakesText.GetSiblingIndex();
                textObj.transform.SetSiblingIndex(mistakesIndex + 1);
            }
            
            currentWordProgressText = textObj.transform;
            sb.AppendLine("Created BotCurrentWordProgressText.");
        }
        else
        {
            sb.AppendLine("BotCurrentWordProgressText already exists.");
        }
        
        // Get or add the BotInfoUI component
        BotInfoUI botInfoUI = prefab.GetComponent<BotInfoUI>();
        if (botInfoUI == null)
        {
            botInfoUI = prefab.AddComponent<BotInfoUI>();
            sb.AppendLine("Added BotInfoUI component.");
        }
        
        // Assign all references using SerializedObject
        SerializedObject serializedObject = new SerializedObject(botInfoUI);
        
        Transform botNameText = prefab.transform.Find("BotNameText");
        Transform botWordText = prefab.transform.Find("BotWordText");
        Transform botMistakesText = prefab.transform.Find("BotMistakesText");
        Transform botWordsCompletedText = prefab.transform.Find("BotWordsCompletedText");
        
        if (botNameText != null)
        {
            serializedObject.FindProperty("botNameText").objectReferenceValue = botNameText.GetComponent<Text>();
            sb.AppendLine("Assigned botNameText.");
        }
        
        if (botWordText != null)
        {
            serializedObject.FindProperty("botWordText").objectReferenceValue = botWordText.GetComponent<Text>();
            sb.AppendLine("Assigned botWordText.");
        }
        
        if (botMistakesText != null)
        {
            serializedObject.FindProperty("botMistakesText").objectReferenceValue = botMistakesText.GetComponent<Text>();
            sb.AppendLine("Assigned botMistakesText.");
        }
        
        if (botWordsCompletedText != null)
        {
            serializedObject.FindProperty("botWordsCompletedText").objectReferenceValue = botWordsCompletedText.GetComponent<Text>();
            sb.AppendLine("Assigned botWordsCompletedText.");
        }
        
        if (currentWordProgressText != null)
        {
            serializedObject.FindProperty("botCurrentWordProgressText").objectReferenceValue = currentWordProgressText.GetComponent<Text>();
            sb.AppendLine("Assigned botCurrentWordProgressText.");
        }
        
        serializedObject.ApplyModifiedProperties();
        
        // Mark the prefab as dirty and save
        EditorUtility.SetDirty(prefab);
        PrefabUtility.SavePrefabAsset(prefab);
        
        sb.AppendLine("\nSuccessfully setup BotInfoUI prefab!");
        
        return sb.ToString();
    }
}
