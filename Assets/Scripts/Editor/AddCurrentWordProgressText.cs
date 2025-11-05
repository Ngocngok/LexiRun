using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AddCurrentWordProgressText : MonoBehaviour
{
    public static string Execute()
    {
        string prefabPath = "Assets/Prefabs/BotInfoUI.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        
        if (prefab == null)
        {
            return "Prefab not found at: " + prefabPath;
        }
        
        // Check if the text element already exists
        Transform existingText = prefab.transform.Find("BotCurrentWordProgressText");
        if (existingText != null)
        {
            return "BotCurrentWordProgressText already exists in the prefab.";
        }
        
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
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
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
        
        // Set sibling index to place it after BotMistakesText
        Transform mistakesText = prefab.transform.Find("BotMistakesText");
        if (mistakesText != null)
        {
            int mistakesIndex = mistakesText.GetSiblingIndex();
            textObj.transform.SetSiblingIndex(mistakesIndex + 1);
        }
        
        // Save the prefab
        PrefabUtility.SavePrefabAsset(prefab);
        
        return "Successfully added BotCurrentWordProgressText to BotInfoUI prefab.";
    }
}
