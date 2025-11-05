using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ActivateBotInfoPanel : MonoBehaviour
{
    public static string Execute()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = activeScene.GetRootGameObjects();
        
        foreach (GameObject root in rootObjects)
        {
            Transform found = root.transform.Find("BotInfoPanel");
            if (found == null)
            {
                // Search recursively
                found = FindInChildren(root.transform, "BotInfoPanel");
            }
            
            if (found != null)
            {
                found.gameObject.SetActive(true);
                EditorUtility.SetDirty(found.gameObject);
                return "Activated BotInfoPanel";
            }
        }
        
        return "BotInfoPanel not found";
    }
    
    private static Transform FindInChildren(Transform parent, string name)
    {
        if (parent.name == name)
            return parent;
            
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform found = FindInChildren(parent.GetChild(i), name);
            if (found != null)
                return found;
        }
        
        return null;
    }
}
