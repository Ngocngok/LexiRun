using UnityEngine;
using UnityEditor;

public class FixTextMeshDepth
{
    [MenuItem("Tools/Fix TextMesh Depth Rendering")]
    public static void Execute()
    {
        // Find all TextMesh components in the LetterNode prefab
        string prefabPath = "Assets/Prefabs/LetterNode.prefab";
        GameObject prefabContents = PrefabUtility.LoadPrefabContents(prefabPath);
        
        TextMesh[] textMeshes = prefabContents.GetComponentsInChildren<TextMesh>(true);
        
        foreach (TextMesh tm in textMeshes)
        {
            MeshRenderer mr = tm.GetComponent<MeshRenderer>();
            if (mr != null && mr.sharedMaterial != null)
            {
                Material mat = mr.sharedMaterial;
                
                // Change shader to one that respects depth
                Shader depthShader = Shader.Find("GUI/Text Shader");
                if (depthShader == null)
                {
                    depthShader = Shader.Find("Unlit/Transparent");
                }
                
                if (depthShader != null)
                {
                    Material newMat = new Material(depthShader);
                    newMat.name = "LetterTextMaterial_DepthFixed";
                    newMat.color = Color.black;
                    newMat.mainTexture = mat.mainTexture;
                    
                    // Set render queue to Geometry to render with depth
                    newMat.renderQueue = 2000;
                    
                    // Save material
                    string matPath = "Assets/Materials/LetterTextMaterial_DepthFixed.mat";
                    AssetDatabase.CreateAsset(newMat, matPath);
                    
                    // Assign to renderer
                    mr.sharedMaterial = newMat;
                    
                    Debug.Log($"Fixed TextMesh on {tm.gameObject.name}");
                }
            }
        }
        
        PrefabUtility.SaveAsPrefabAsset(prefabContents, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabContents);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("TextMesh depth rendering fixed!");
    }
}
