using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class ApplyFontToAllText : MonoBehaviour
{
    [MenuItem("LexiRun/Apply Font and Outline to All Text")]
    public static void ApplyFont()
    {
        // Load the font
        Font lilitaFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Fonts/LilitaOne-Regular.ttf");
        
        if (lilitaFont == null)
        {
            Debug.LogError("LilitaOne-Regular font not found!");
            return;
        }
        
        // Get all scene paths
        string[] scenePaths = new string[]
        {
            "Assets/Scenes/LoadingScene.unity",
            "Assets/Scenes/HomeScene.unity",
            "Assets/Scenes/GameplayScene.unity"
        };
        
        int totalTextComponents = 0;
        
        foreach (string scenePath in scenePaths)
        {
            // Open the scene
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            
            // Find all Text components in the scene
            Text[] allTexts = FindObjectsByType<Text>(FindObjectsSortMode.None);
            
            foreach (Text text in allTexts)
            {
                // Apply font
                text.font = lilitaFont;
                
                // Add or update Outline component
                Outline outline = text.GetComponent<Outline>();
                if (outline == null)
                {
                    outline = text.gameObject.AddComponent<Outline>();
                }
                
                // Set outline properties
                outline.effectColor = new Color(0f, 0f, 0f, 0.7f); // Black with alpha 0.7
                outline.effectDistance = new Vector2(2f, -2f); // x=2, y=-2
                
                EditorUtility.SetDirty(text);
                EditorUtility.SetDirty(outline);
                
                totalTextComponents++;
                Debug.Log($"Applied font and outline to: {GetGameObjectPath(text.gameObject)} in {scene.name}");
            }
            
            // Save the scene
            EditorSceneManager.SaveScene(scene);
        }
        
        Debug.Log($"Successfully applied LilitaOne-Regular font and outline to {totalTextComponents} text components across all scenes!");
    }
    
    private static string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform parent = obj.transform.parent;
        
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        
        return path;
    }
}
