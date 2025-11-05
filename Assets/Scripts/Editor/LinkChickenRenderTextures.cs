using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class LinkChickenRenderTextures
{
    [MenuItem("Tools/Link Chicken RenderTextures")]
    static void Link()
    {
        // Load RenderTextures
        RenderTexture winRT = AssetDatabase.LoadAssetAtPath<RenderTexture>("Assets/RenderTextures/WinChickenRT.renderTexture");
        RenderTexture loseRT = AssetDatabase.LoadAssetAtPath<RenderTexture>("Assets/RenderTextures/LoseChickenRT.renderTexture");
        
        if (winRT == null || loseRT == null)
        {
            Debug.LogError("RenderTextures not found! Make sure they exist in Assets/RenderTextures/");
            return;
        }
        
        // Find and link Win screen
        GameObject winDisplay = GameObject.Find("Canvas/VictoryPanel/WinChickenDisplay");
        if (winDisplay != null)
        {
            RawImage rawImage = winDisplay.GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.texture = winRT;
                EditorUtility.SetDirty(winDisplay);
                Debug.Log("Linked WinChickenRT to VictoryPanel");
            }
        }
        
        // Find and link Lose screen
        GameObject loseDisplay = GameObject.Find("Canvas/LosePanel/LoseChickenDisplay");
        if (loseDisplay != null)
        {
            RawImage rawImage = loseDisplay.GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.texture = loseRT;
                EditorUtility.SetDirty(loseDisplay);
                Debug.Log("Linked LoseChickenRT to LosePanel");
            }
        }
        
        // Link cameras to RenderTextures
        GameObject winCam = GameObject.Find("WinChickenScene/WinChickenCamera");
        if (winCam != null)
        {
            Camera cam = winCam.GetComponent<Camera>();
            if (cam != null)
            {
                cam.targetTexture = winRT;
                EditorUtility.SetDirty(winCam);
                Debug.Log("Linked WinChickenCamera to WinChickenRT");
            }
        }
        
        GameObject loseCam = GameObject.Find("LoseChickenScene/LoseChickenCamera");
        if (loseCam != null)
        {
            Camera cam = loseCam.GetComponent<Camera>();
            if (cam != null)
            {
                cam.targetTexture = loseRT;
                EditorUtility.SetDirty(loseCam);
                Debug.Log("Linked LoseChickenCamera to LoseChickenRT");
            }
        }
        
        Debug.Log("All RenderTextures linked successfully!");
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
}
