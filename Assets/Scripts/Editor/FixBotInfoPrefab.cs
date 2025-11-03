using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class FixBotInfoPrefab
{
    [MenuItem("LexiRun/Fix BotInfo Prefab")]
    public static void Fix()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BotInfoUI.prefab");
        
        if (prefab != null)
        {
            GameObject prefabInstance = PrefabUtility.LoadPrefabContents("Assets/Prefabs/BotInfoUI.prefab");
            
            BotInfoUI botInfo = prefabInstance.GetComponent<BotInfoUI>();
            if (botInfo == null)
            {
                botInfo = prefabInstance.AddComponent<BotInfoUI>();
            }
            
            botInfo.botNameText = prefabInstance.transform.Find("BotNameText").GetComponent<Text>();
            botInfo.botWordText = prefabInstance.transform.Find("BotWordText").GetComponent<Text>();
            botInfo.botMistakesText = prefabInstance.transform.Find("BotMistakesText").GetComponent<Text>();
            botInfo.botWordsCompletedText = prefabInstance.transform.Find("BotWordsCompletedText").GetComponent<Text>();
            
            PrefabUtility.SaveAsPrefabAsset(prefabInstance, "Assets/Prefabs/BotInfoUI.prefab");
            PrefabUtility.UnloadPrefabContents(prefabInstance);
            
            Debug.Log("BotInfoUI prefab fixed!");
        }
    }
}
