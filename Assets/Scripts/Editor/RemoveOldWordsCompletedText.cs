using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class RemoveOldWordsCompletedText : MonoBehaviour
{
    public static string Execute()
    {
        string prefabPath = "Assets/Prefabs/BotInfoUI.prefab";
        
        // Load prefab contents
        GameObject prefabContents = PrefabUtility.LoadPrefabContents(prefabPath);
        
        try
        {
            // Remove BotWordsCompletedText
            Transform oldText = prefabContents.transform.Find("BotWordsCompletedText");
            if (oldText != null)
            {
                Object.DestroyImmediate(oldText.gameObject);
            }
            
            // Update BotInfoUI component references
            BotInfoUI botInfoUI = prefabContents.GetComponent<BotInfoUI>();
            if (botInfoUI != null)
            {
                // Re-assign references (botWordsCompletedText will be null now)
                botInfoUI.botNameText = prefabContents.transform.Find("BotNameText")?.GetComponent<Text>();
                botInfoUI.botWordText = prefabContents.transform.Find("BotWordText")?.GetComponent<Text>();
                botInfoUI.botMistakesText = prefabContents.transform.Find("BotMistakesText")?.GetComponent<Text>();
                botInfoUI.botCurrentWordProgressText = prefabContents.transform.Find("BotCurrentWordProgressText")?.GetComponent<Text>();
            }
            
            // Save
            PrefabUtility.SaveAsPrefabAsset(prefabContents, prefabPath);
            
            return "Removed BotWordsCompletedText from prefab successfully!";
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefabContents);
        }
    }
}
