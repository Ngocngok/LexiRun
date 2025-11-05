using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets up RenderTextures for Win/Lose chicken displays at runtime.
/// Creates RenderTextures dynamically and assigns them to cameras and UI.
/// </summary>
public class ChickenRenderTextureSetup : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera winChickenCamera;
    [SerializeField] private Camera loseChickenCamera;
    
    [Header("UI RawImages")]
    [SerializeField] private RawImage winChickenDisplay;
    [SerializeField] private RawImage loseChickenDisplay;
    
    [Header("Settings")]
    [SerializeField] private int textureSize = 512;
    [SerializeField] private int antiAliasing = 4;
    
    private RenderTexture winRT;
    private RenderTexture loseRT;
    
    void Awake()
    {
        SetupRenderTextures();
    }
    
    void SetupRenderTextures()
    {
        // Create Win RenderTexture
        winRT = new RenderTexture(textureSize, textureSize, 24);
        winRT.name = "WinChickenRT";
        winRT.antiAliasing = antiAliasing;
        
        // Create Lose RenderTexture
        loseRT = new RenderTexture(textureSize, textureSize, 24);
        loseRT.name = "LoseChickenRT";
        loseRT.antiAliasing = antiAliasing;
        
        // Assign to cameras
        if (winChickenCamera != null)
        {
            winChickenCamera.targetTexture = winRT;
        }
        else
        {
            Debug.LogWarning("WinChickenCamera not assigned!");
        }
        
        if (loseChickenCamera != null)
        {
            loseChickenCamera.targetTexture = loseRT;
        }
        else
        {
            Debug.LogWarning("LoseChickenCamera not assigned!");
        }
        
        // Assign to UI
        if (winChickenDisplay != null)
        {
            winChickenDisplay.texture = winRT;
        }
        else
        {
            Debug.LogWarning("WinChickenDisplay not assigned!");
        }
        
        if (loseChickenDisplay != null)
        {
            loseChickenDisplay.texture = loseRT;
        }
        else
        {
            Debug.LogWarning("LoseChickenDisplay not assigned!");
        }
        
        Debug.Log("Chicken RenderTextures setup complete!");
    }
    
    void OnDestroy()
    {
        // Clean up RenderTextures
        if (winRT != null)
        {
            winRT.Release();
            Destroy(winRT);
        }
        
        if (loseRT != null)
        {
            loseRT.Release();
            Destroy(loseRT);
        }
    }
}
