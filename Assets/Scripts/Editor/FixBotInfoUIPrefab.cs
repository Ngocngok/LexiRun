using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class FixBotInfoUIPrefab : MonoBehaviour
{
    public static string Execute()
    {
        string prefabPath = "Assets/Prefabs/BotInfoUI.prefab";
        
        // Load prefab contents
        GameObject prefabContents = PrefabUtility.LoadPrefabContents(prefabPath);
        
        try
        {
            // Remove all missing scripts
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(prefabContents);
            
            // Remove existing BotCurrentWordProgressText if it exists
            Transform existing = prefabContents.transform.Find("BotCurrentWordProgressText");
            if (existing != null)
            {
                Object.DestroyImmediate(existing.gameObject);
            }
            
            // Create new text object
            GameObject textObj = new GameObject("BotCurrentWordProgressText");
            textObj.transform.SetParent(prefabContents.transform, false);
            
            // Setup RectTransform
            RectTransform rt = textObj.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.sizeDelta = new Vector2(0, 30);
            rt.anchoredPosition = Vector2.zero;
            
            // Setup Text component
            Text text = textObj.AddComponent<Text>();
            text.text = "Current: 0/5";
            text.fontSize = 12;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleLeft;
            
            // Copy font from BotMistakesText
            Transform mistakesText = prefabContents.transform.Find("BotMistakesText");
            if (mistakesText != null)
            {
                Text mistakesTextComp = mistakesText.GetComponent<Text>();
                if (mistakesTextComp != null && mistakesTextComp.font != null)
                {
                    text.font = mistakesTextComp.font;
                    text.fontSize = mistakesTextComp.fontSize;
                }
                
                // Place after BotMistakesText
                textObj.transform.SetSiblingIndex(mistakesText.GetSiblingIndex() + 1);
            }
            
            // Add BotInfoUI component
            BotInfoUI botInfoUI = prefabContents.AddComponent<BotInfoUI>();
            
            // Assign all references
            botInfoUI.botNameText = prefabContents.transform.Find("BotNameText")?.GetComponent<Text>();
            botInfoUI.botWordText = prefabContents.transform.Find("BotWordText")?.GetComponent<Text>();
            botInfoUI.botMistakesText = prefabContents.transform.Find("BotMistakesText")?.GetComponent<Text>();
            botInfoUI.botCurrentWordProgressText = textObj.GetComponent<Text>();
            
            // Save
            PrefabUtility.SaveAsPrefabAsset(prefabContents, prefabPath);
            
            return "BotInfoUI prefab fixed and setup complete!";
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefabContents);
        }
    }
}
