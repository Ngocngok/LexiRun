using UnityEngine;
using UnityEditor;

/// <summary>
/// Fixes the TextMesh rendering issue on LetterNodes where text renders above opaque objects.
/// Sets the material's render queue to respect depth properly.
/// </summary>
public class FixLetterNodeTextDepth
{
    [MenuItem("Tools/Fix Letter Node Text Depth")]
    static void Fix()
    {
        // Load the LetterNode prefab
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/LetterNode.prefab");
        
        if (prefab == null)
        {
            Debug.LogError("LetterNode prefab not found!");
            return;
        }
        
        // Open prefab for editing
        string prefabPath = "Assets/Prefabs/LetterNode.prefab";
        GameObject prefabInstance = PrefabUtility.LoadPrefabContents(prefabPath);
        
        // Find the LetterText child
        Transform letterTextTransform = prefabInstance.transform.Find("LetterText");
        
        if (letterTextTransform == null)
        {
            Debug.LogError("LetterText not found in prefab!");
            PrefabUtility.UnloadPrefabContents(prefabInstance);
            return;
        }
        
        // Get the TextMesh component
        TextMesh textMesh = letterTextTransform.GetComponent<TextMesh>();
        MeshRenderer meshRenderer = letterTextTransform.GetComponent<MeshRenderer>();
        
        if (textMesh == null || meshRenderer == null)
        {
            Debug.LogError("TextMesh or MeshRenderer not found!");
            PrefabUtility.UnloadPrefabContents(prefabInstance);
            return;
        }
        
        // Get or create material
        Material textMaterial = meshRenderer.sharedMaterial;
        
        if (textMaterial != null)
        {
            // Create a new material instance to avoid modifying the shared font material
            Material newMaterial = new Material(textMaterial);
            newMaterial.name = "LetterNodeTextMaterial";
            
            // Set render queue to Geometry (2000) to respect depth
            // Default TextMesh materials often use Transparent (3000) which renders on top
            newMaterial.renderQueue = 2000; // Geometry queue
            
            // Enable ZWrite to write to depth buffer
            if (newMaterial.HasProperty("_ZWrite"))
            {
                newMaterial.SetInt("_ZWrite", 1);
            }
            
            // Save material as asset
            string materialPath = "Assets/Materials/LetterNodeTextMaterial.mat";
            AssetDatabase.CreateAsset(newMaterial, materialPath);
            
            // Assign to mesh renderer
            meshRenderer.sharedMaterial = newMaterial;
            
            Debug.Log($"Created and assigned material: {materialPath}");
        }
        
        // Save prefab changes
        PrefabUtility.SaveAsPrefabAsset(prefabInstance, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabInstance);
        
        Debug.Log("Letter Node text depth fixed! Text will now render properly behind opaque objects.");
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
