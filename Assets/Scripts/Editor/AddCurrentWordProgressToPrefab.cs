using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Text;

public class AddCurrentWordProgressToPrefab : MonoBehaviour
{
    public static string Execute()
    {
        string prefabPath = "Assets/Prefabs/BotInfoUI.prefab";
        
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Adding Current Word Progress to BotInfoUI prefab...");
        
        // Load the prefab
        GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefabAsset == null)
        {
            return "ERROR: Prefab not found at: " + prefabPath;
        }
        
        // Load the prefab contents for editing
        string tempPath = prefabPath;
        GameObject prefabContents = PrefabUtility.LoadPrefabContents(tempPath);
        
        if (prefabContents == null)
        {
            return "ERROR: Could not load prefab contents.";
        }
        
        try
        {
            // Check if BotCurrentWordProgressText already exists
            Transform existingText = prefabContents.transform.Find("BotCurrentWordProgressText");
            if (existingText != null)
            {
                sb.AppendLine("BotCurrentWordProgressText already exists. Removing it first...");
                GameObject.DestroyImmediate(existingText.gameObject);
            }
            
            // Create a new GameObject for the current word progress text
            GameObject textObj = new GameObject("BotCurrentWordProgressText");
            textObj.transform.SetParent(prefabContents.transform, false);
            
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
            Text referenceText = prefabContents.transform.Find("BotMistakesText")?.GetComponent<Text>();
            if (referenceText != null && referenceText.font != null)
            {
                text.font = referenceText.font;
                text.fontSize = referenceText.fontSize;
                sb.AppendLine("Using font from BotMistakesText.");
            }
            else
            {
                text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                sb.AppendLine("Using default font.");
            }
            
            // Set sibling index to place it after BotMistakesText
            Transform mistakesText = prefabContents.transform.Find("BotMistakesText");
            if (mistakesText != null)
            {
                int mistakesIndex = mistakesText.GetSiblingIndex();
                textObj.transform.SetSiblingIndex(mistakesIndex + 1);
                sb.AppendLine($"Set sibling index to {mistakesIndex + 1}.");
            }
            
            sb.AppendLine("Created BotCurrentWordProgressText.");
            
            // Get or add the BotInfoUI component
            BotInfoUI botInfoUI = prefabContents.GetComponent<BotInfoUI>();
            if (botInfoUI == null)
            {
                botInfoUI = prefabContents.AddComponent<BotInfoUI>();
                sb.AppendLine("Added BotInfoUI component.");
            }
            else
            {
                sb.AppendLine("BotInfoUI component already exists.");
            }
            
            // Assign all references
            Transform botNameText = prefabContents.transform.Find("BotNameText");
            Transform botWordText = prefabContents.transform.Find("BotWordText");
            Transform botMistakesText2 = prefabContents.transform.Find("BotMistakesText");
            Transform botWordsCompletedText = prefabContents.transform.Find("BotWordsCompletedText");
            Transform botCurrentWordProgressText = prefabContents.transform.Find("BotCurrentWordProgressText");
            
            if (botNameText != null) botInfoUI.botNameText = botNameText.GetComponent<Text>();
            if (botWordText != null) botInfoUI.botWordText = botWordText.GetComponent<Text>();
            if (botMistakesText2 != null) botInfoUI.botMistakesText = botMistakesText2.GetComponent<Text>();
            if (botCurrentWordProgressText != null) botInfoUI.botCurrentWordProgressText = botCurrentWordProgressText.GetComponent<Text>();
            
            sb.AppendLine("Assigned all references.");
            
            // Save the prefab
            PrefabUtility.SaveAsPrefabAsset(prefabContents, tempPath);
            sb.AppendLine("Saved prefab.");
            
            sb.AppendLine("\nSUCCESS! BotInfoUI prefab updated.");
        }
        finally
        {
            // Unload the prefab contents
            PrefabUtility.UnloadPrefabContents(prefabContents);
        }
        
        return sb.ToString();
    }
}
