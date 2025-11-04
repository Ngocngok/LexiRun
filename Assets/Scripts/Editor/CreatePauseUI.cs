using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CreatePauseUI : MonoBehaviour
{
    [MenuItem("LexiRun/Create Pause UI")]
    public static void Create()
    {
        // Find canvas
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }
        
        // Load font
        Font lilitaFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Resources/LilitaOne-Regular.ttf");
        
        // Create Pause Button (top-right corner)
        GameObject pauseButton = CreatePauseButton(canvas.transform, lilitaFont);
        
        // Create Pause Popup Panel
        GameObject pausePanel = CreatePausePanel(canvas.transform, lilitaFont);
        
        // Update UIManager reference
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            SerializedObject so = new SerializedObject(uiManager);
            so.FindProperty("pauseButton").objectReferenceValue = pauseButton.GetComponent<Button>();
            so.FindProperty("pausePanel").objectReferenceValue = pausePanel;
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(uiManager);
        }
        
        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        
        Debug.Log("Pause UI created successfully!");
    }
    
    private static GameObject CreatePauseButton(Transform parent, Font font)
    {
        // Create button
        GameObject buttonObj = new GameObject("PauseButton");
        buttonObj.transform.SetParent(parent, false);
        
        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1); // Top-right
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(-20, -20);
        rect.sizeDelta = new Vector2(80, 80);
        
        // Add Image
        Image image = buttonObj.AddComponent<Image>();
        Sprite pauseSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Sprite/Demo/Demo_Icon/Icon_PictoIcon_Pause.png");
        image.sprite = pauseSprite;
        image.preserveAspect = true;
        
        // Add Button
        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = image;
        
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f);
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f);
        button.colors = colors;
        
        return buttonObj;
    }
    
    private static GameObject CreatePausePanel(Transform parent, Font font)
    {
        // Create overlay
        GameObject overlay = new GameObject("PauseOverlay");
        overlay.transform.SetParent(parent, false);
        
        RectTransform overlayRect = overlay.AddComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;
        
        Image overlayImage = overlay.AddComponent<Image>();
        overlayImage.color = new Color(0, 0, 0, 0.8f); // Dark overlay
        
        // Create panel
        GameObject panel = new GameObject("PausePanel");
        panel.transform.SetParent(overlay.transform, false);
        
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(500, 400);
        
        Image panelImage = panel.AddComponent<Image>();
        Sprite panelSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Sprite/Component/Popup_Frame_01.png");
        panelImage.sprite = panelSprite;
        panelImage.type = Image.Type.Sliced;
        
        // Create title
        GameObject title = new GameObject("Title");
        title.transform.SetParent(panel.transform, false);
        
        RectTransform titleRect = title.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1);
        titleRect.anchorMax = new Vector2(0.5f, 1);
        titleRect.pivot = new Vector2(0.5f, 1);
        titleRect.anchoredPosition = new Vector2(0, -40);
        titleRect.sizeDelta = new Vector2(400, 80);
        
        Text titleText = title.AddComponent<Text>();
        titleText.text = "PAUSED";
        titleText.font = font != null ? font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 60;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = Color.white;
        
        Outline titleOutline = title.AddComponent<Outline>();
        titleOutline.effectColor = new Color(0, 0, 0, 0.7f);
        titleOutline.effectDistance = new Vector2(2, -2);
        
        // Create buttons container
        GameObject buttonsContainer = new GameObject("ButtonsContainer");
        buttonsContainer.transform.SetParent(panel.transform, false);
        
        RectTransform containerRect = buttonsContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.pivot = new Vector2(0.5f, 0.5f);
        containerRect.anchoredPosition = new Vector2(0, -20);
        containerRect.sizeDelta = new Vector2(400, 200);
        
        VerticalLayoutGroup layout = buttonsContainer.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 20;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        
        // Create Resume button
        GameObject resumeButton = CreateButton("ResumeButton", "Resume", buttonsContainer.transform, font);
        
        // Create Home button
        GameObject homeButton = CreateButton("HomeButton", "Back to Home", buttonsContainer.transform, font);
        
        // Hide panel by default
        overlay.SetActive(false);
        
        return overlay;
    }
    
    private static GameObject CreateButton(string name, string text, Transform parent, Font font)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);
        
        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(300, 80);
        
        Image image = buttonObj.AddComponent<Image>();
        Sprite buttonSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Sprite/Component/Button_01.png");
        image.sprite = buttonSprite;
        image.type = Image.Type.Sliced;
        
        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = image;
        
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f);
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f);
        button.colors = colors;
        
        // Create text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        Text buttonText = textObj.AddComponent<Text>();
        buttonText.text = text;
        buttonText.font = font != null ? font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        buttonText.fontSize = 32;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;
        
        Outline textOutline = textObj.AddComponent<Outline>();
        textOutline.effectColor = new Color(0, 0, 0, 0.7f);
        textOutline.effectDistance = new Vector2(2, -2);
        
        return buttonObj;
    }
}
