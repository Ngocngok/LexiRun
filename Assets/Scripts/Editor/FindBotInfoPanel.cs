using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class FindBotInfoPanel : MonoBehaviour
{
    public static string Execute()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = activeScene.GetRootGameObjects();
        
        foreach (GameObject root in rootObjects)
        {
            Transform found = FindInChildren(root.transform, "BotInfoPanel");
            if (found != null)
            {
                return "Found BotInfoPanel at: " + GetFullPath(found);
            }
        }
        
        return "BotInfoPanel not found in the scene.";
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
    
    private static string GetFullPath(Transform transform)
    {
        string path = transform.name;
        Transform current = transform.parent;
        
        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }
        
        return "/" + path;
    }
}
