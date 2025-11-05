using UnityEngine;
using UnityEditor;
using System.Text;

public class ListPrefabHierarchy : MonoBehaviour
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
        sb.AppendLine("BotInfoUI Prefab Hierarchy:");
        sb.AppendLine("==========================");
        
        ListChildren(prefab.transform, sb, 0);
        
        return sb.ToString();
    }
    
    private static void ListChildren(Transform parent, StringBuilder sb, int depth)
    {
        string indent = new string(' ', depth * 2);
        
        sb.AppendLine(indent + parent.name);
        
        // List components
        Component[] components = parent.GetComponents<Component>();
        foreach (Component comp in components)
        {
            if (comp != null && !(comp is Transform))
            {
                sb.AppendLine(indent + "  - " + comp.GetType().Name);
            }
        }
        
        // List children
        for (int i = 0; i < parent.childCount; i++)
        {
            ListChildren(parent.GetChild(i), sb, depth + 1);
        }
    }
}
