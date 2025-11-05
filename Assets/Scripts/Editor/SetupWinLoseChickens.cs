using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SetupWinLoseChickens : EditorWindow
{
    [MenuItem("Tools/Setup Win/Lose Chickens")]
    static void Setup()
    {
        // Create RenderTextures
        RenderTexture winRT = CreateRenderTexture("WinChickenRT");
        RenderTexture loseRT = CreateRenderTexture("LoseChickenRT");
        
        // Setup Win Screen
        SetupWinScreen(winRT);
        
        // Setup Lose Screen
        SetupLoseScreen(loseRT);
        
        Debug.Log("Win/Lose Chickens setup complete!");
        EditorUtility.SetDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[0]);
    }
    
    static RenderTexture CreateRenderTexture(string name)
    {
        string path = $"Assets/RenderTextures/{name}.renderTexture";
        
        // Check if already exists
        RenderTexture existing = AssetDatabase.LoadAssetAtPath<RenderTexture>(path);
        if (existing != null)
        {
            Debug.Log($"Using existing RenderTexture: {name}");
            return existing;
        }
        
        // Create new RenderTexture
        RenderTexture rt = new RenderTexture(512, 512, 24);
        rt.name = name;
        rt.antiAliasing = 4;
        
        // Save as asset
        AssetDatabase.CreateAsset(rt, path);
        AssetDatabase.SaveAssets();
        
        Debug.Log($"Created RenderTexture: {name}");
        return rt;
    }
    
    static void SetupWinScreen(RenderTexture renderTexture)
    {
        // Find VictoryPanel
        GameObject victoryPanel = GameObject.Find("Canvas/VictoryPanel");
        if (victoryPanel == null)
        {
            Debug.LogError("VictoryPanel not found!");
            return;
        }
        
        // Create chicken display container
        GameObject winChickenContainer = new GameObject("WinChickenDisplay");
        winChickenContainer.transform.SetParent(victoryPanel.transform, false);
        winChickenContainer.transform.SetSiblingIndex(0); // Put at top
        
        // Add RawImage
        RawImage rawImage = winChickenContainer.AddComponent<RawImage>();
        rawImage.texture = renderTexture;
        
        // Setup RectTransform
        RectTransform rt = winChickenContainer.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.anchoredPosition = new Vector2(0, -50);
        rt.sizeDelta = new Vector2(300, 300);
        
        // Create 3D scene for rendering
        GameObject winScene = new GameObject("WinChickenScene");
        winScene.transform.position = new Vector3(1000, 0, 0); // Far away from main scene
        
        // Create camera
        GameObject camObj = new GameObject("WinChickenCamera");
        camObj.transform.SetParent(winScene.transform);
        camObj.transform.localPosition = new Vector3(0, 1.5f, -2.5f);
        camObj.transform.localRotation = Quaternion.Euler(45, 0, 0);
        
        Camera cam = camObj.AddComponent<Camera>();
        cam.targetTexture = renderTexture;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0);
        cam.cullingMask = 1 << LayerMask.NameToLayer("Default");
        cam.fieldOfView = 30; // Portrait view
        
        // Instantiate chicken from prefab
        GameObject chickenPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Quirky Series Ultimate/Quirky Series Vol.1/Farm Vol.1/Models/Chick_LOD0.fbx");
        if (chickenPrefab != null)
        {
            GameObject chicken = PrefabUtility.InstantiatePrefab(chickenPrefab) as GameObject;
            chicken.name = "WinChicken";
            chicken.transform.SetParent(winScene.transform);
            chicken.transform.localPosition = Vector3.zero;
            chicken.transform.localRotation = Quaternion.identity;
            chicken.transform.localScale = Vector3.one;
            
            // Add controller
            ChickenDisplayController controller = chicken.AddComponent<ChickenDisplayController>();
            SerializedObject so = new SerializedObject(controller);
            so.FindProperty("displayType").enumValueIndex = 0; // Victory
            so.ApplyModifiedProperties();
        }
        
        Debug.Log("Win screen chicken setup complete!");
    }
    
    static void SetupLoseScreen(RenderTexture renderTexture)
    {
        // Find LosePanel
        GameObject losePanel = GameObject.Find("Canvas/LosePanel");
        if (losePanel == null)
        {
            Debug.LogError("LosePanel not found!");
            return;
        }
        
        // Create chicken display container
        GameObject loseChickenContainer = new GameObject("LoseChickenDisplay");
        loseChickenContainer.transform.SetParent(losePanel.transform, false);
        loseChickenContainer.transform.SetSiblingIndex(0); // Put at top
        
        // Add RawImage
        RawImage rawImage = loseChickenContainer.AddComponent<RawImage>();
        rawImage.texture = renderTexture;
        
        // Setup RectTransform
        RectTransform rt = loseChickenContainer.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.anchoredPosition = new Vector2(0, -50);
        rt.sizeDelta = new Vector2(300, 300);
        
        // Create 3D scene for rendering
        GameObject loseScene = new GameObject("LoseChickenScene");
        loseScene.transform.position = new Vector3(2000, 0, 0); // Far away from main scene
        
        // Create camera
        GameObject camObj = new GameObject("LoseChickenCamera");
        camObj.transform.SetParent(loseScene.transform);
        camObj.transform.localPosition = new Vector3(0, 1.5f, -2.5f);
        camObj.transform.localRotation = Quaternion.Euler(45, 0, 0);
        
        Camera cam = camObj.AddComponent<Camera>();
        cam.targetTexture = renderTexture;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0);
        cam.cullingMask = 1 << LayerMask.NameToLayer("Default");
        cam.fieldOfView = 30; // Portrait view
        
        // Instantiate chicken from prefab
        GameObject chickenPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Quirky Series Ultimate/Quirky Series Vol.1/Farm Vol.1/Models/Chick_LOD0.fbx");
        if (chickenPrefab != null)
        {
            GameObject chicken = PrefabUtility.InstantiatePrefab(chickenPrefab) as GameObject;
            chicken.name = "LoseChicken";
            chicken.transform.SetParent(loseScene.transform);
            chicken.transform.localPosition = Vector3.zero;
            chicken.transform.localRotation = Quaternion.identity;
            chicken.transform.localScale = Vector3.one;
            
            // Add controller
            ChickenDisplayController controller = chicken.AddComponent<ChickenDisplayController>();
            SerializedObject so = new SerializedObject(controller);
            so.FindProperty("displayType").enumValueIndex = 1; // Defeat
            so.ApplyModifiedProperties();
        }
        
        Debug.Log("Lose screen chicken setup complete!");
    }
}
