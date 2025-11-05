using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UpdateBotInfoUIText : MonoBehaviour
{
    public static string Execute()
    {
        string prefabPath = "Assets/Prefabs/BotInfoUI.prefab";
        
        // Load prefab contents
        GameObject prefabContents = PrefabUtility.LoadPrefabContents(prefabPath);
        
        try
        {
            // Find the BotCurrentWordProgressText
            Transform textTransform = prefabContents.transform.Find("BotCurrentWordProgressText");
            if (textTransform != null)
            {
                Text textComponent = textTransform.GetComponent<Text>();
                if (textComponent != null)
                {
                    textComponent.text = "0/3";
                }
            }
            
            // Save
            PrefabUtility.SaveAsPrefabAsset(prefabContents, prefabPath);
            
            return "Updated BotCurrentWordProgressText default text to '0/3'";
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefabContents);
        }
    }
}
