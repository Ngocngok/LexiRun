using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CreateTutorialUI : MonoBehaviour
{
    [MenuItem("LexiRun/Create Tutorial UI")]
    public static void Create()
    {
        // Find canvas
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }
        
        // Load font
        Font lilitaFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Resources/LilitaOne-Regular.ttf");
        
        // Delete old tutorial if exists
        GameObject oldTutorial = GameObject.Find("Canvas/TutorialOverlay");
        if (oldTutorial != null)
        {
            DestroyImmediate(oldTutorial);
        }
        
        // Create tutorial overlay
        GameObject tutorialOverlay = CreateTutorialOverlay(canvas.transform, lilitaFont);
        
        // Update UIManager reference
        UIManager uiManager = canvas.GetComponent<UIManager>();
        if (uiManager != null)
        {
            SerializedObject so = new SerializedObject(uiManager);
            so.FindProperty("tutorialPanel").objectReferenceValue = tutorialOverlay;
            
            // Find and assign tutorial elements
            GameObject slide1 = GameObject.Find("Canvas/TutorialOverlay/TutorialPanel/Slide1");
            GameObject slide2 = GameObject.Find("Canvas/TutorialOverlay/TutorialPanel/Slide2");
            GameObject slide3 = GameObject.Find("Canvas/TutorialOverlay/TutorialPanel/Slide3");
            
            if (slide1 != null)
            {
                so.FindProperty("tutorialSlide1").objectReferenceValue = slide1;
                GameObject img1 = GameObject.Find("Canvas/TutorialOverlay/TutorialPanel/Slide1/Image");
                GameObject next1 = GameObject.Find("Canvas/TutorialOverlay/TutorialPanel/Slide1/NextButton");
                if (img1 != null) so.FindProperty("tutorialImage1").objectReferenceValue = img1.GetComponent<Image>();
                if (next1 != null) so.FindProperty("tutorialNextButton1").objectReferenceValue = next1.GetComponent<Button>();
            }
            
            if (slide2 != null)
            {
                so.FindProperty("tutorialSlide2").objectReferenceValue = slide2;
                GameObject img2 = GameObject.Find("Canvas/TutorialOverlay/TutorialPanel/Slide2/Image");
                GameObject next2 = GameObject.Find("Canvas/TutorialOverlay/TutorialPanel/Slide2/NextButton");
                if (img2 != null) so.FindProperty("tutorialImage2").objectReferenceValue = img2.GetComponent<Image>();
                if (next2 != null) so.FindProperty("tutorialNextButton2").objectReferenceValue = next2.GetComponent<Button>();
            }
            
            if (slide3 != null)
            {
                so.FindProperty("tutorialSlide3").objectReferenceValue = slide3;
                GameObject img3 = GameObject.Find("Canvas/TutorialOverlay/TutorialPanel/Slide3/Image");
                GameObject ok = GameObject.Find("Canvas/TutorialOverlay/TutorialPanel/Slide3/OKButton");
                if (img3 != null) so.FindProperty("tutorialImage3").objectReferenceValue = img3.GetComponent<Image>();
                if (ok != null) so.FindProperty("tutorialOKButton").objectReferenceValue = ok.GetComponent<Button>();
            }
            
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(uiManager);
        }
        
        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        
        Debug.Log("Tutorial UI created successfully!");
    }
    
    private static GameObject CreateTutorialOverlay(Transform parent, Font font)
    {
        // Create overlay
        GameObject overlay = new GameObject("TutorialOverlay");
        overlay.transform.SetParent(parent, false);
        
        RectTransform overlayRect = overlay.AddComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;
        
        Image overlayImage = overlay.AddComponent<Image>();
        overlayImage.color = new Color(0, 0, 0, 0.9f); // Very dark overlay
        
        // Create panel
        GameObject panel = new GameObject("TutorialPanel");
        panel.transform.SetParent(overlay.transform, false);
        
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(800, 1200);
        
        Image panelImage = panel.AddComponent<Image>();
        Sprite panelSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Sprite/Component/Popup_Frame_01.png");
        if (panelSprite != null)
        {
            panelImage.sprite = panelSprite;
            panelImage.type = Image.Type.Sliced;
        }
        else
        {
            panelImage.color = new Color(0.2f, 0.3f, 0.5f, 1f);
        }
        
        // Create 3 slides
        CreateSlide1(panel.transform, font);
        CreateSlide2(panel.transform, font);
        CreateSlide3(panel.transform, font);
        
        // Hide overlay by default
        overlay.SetActive(false);
        
        return overlay;
    }
    
    private static void CreateSlide1(Transform parent, Font font)
    {
        GameObject slide = new GameObject("Slide1");
        slide.transform.SetParent(parent, false);
        
        RectTransform slideRect = slide.AddComponent<RectTransform>();
        slideRect.anchorMin = Vector2.zero;
        slideRect.anchorMax = Vector2.one;
        slideRect.offsetMin = Vector2.zero;
        slideRect.offsetMax = Vector2.zero;
        
        // Create image placeholder
        GameObject imageObj = new GameObject("Image");
        imageObj.transform.SetParent(slide.transform, false);
        
        RectTransform imageRect = imageObj.AddComponent<RectTransform>();
        imageRect.anchorMin = new Vector2(0.5f, 0.5f);
        imageRect.anchorMax = new Vector2(0.5f, 0.5f);
        imageRect.pivot = new Vector2(0.5f, 0.5f);
        imageRect.anchoredPosition = new Vector2(0, 150);
        imageRect.sizeDelta = new Vector2(600, 600);
        
        Image image = imageObj.AddComponent<Image>();
        image.color = new Color(0.8f, 0.8f, 0.8f, 0.3f); // Light gray placeholder
        
        // Create text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(slide.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = new Vector2(0, -300);
        textRect.sizeDelta = new Vector2(700, 150);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "Drag to move around";
        text.font = font != null ? font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 48;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = new Color(0, 0, 0, 0.7f);
        outline.effectDistance = new Vector2(2, -2);
        
        // Create Next button
        CreateButton("NextButton", "Next", new Vector2(0, -480), slide.transform, font);
    }
    
    private static void CreateSlide2(Transform parent, Font font)
    {
        GameObject slide = new GameObject("Slide2");
        slide.transform.SetParent(parent, false);
        
        RectTransform slideRect = slide.AddComponent<RectTransform>();
        slideRect.anchorMin = Vector2.zero;
        slideRect.anchorMax = Vector2.one;
        slideRect.offsetMin = Vector2.zero;
        slideRect.offsetMax = Vector2.zero;
        
        // Create image placeholder
        GameObject imageObj = new GameObject("Image");
        imageObj.transform.SetParent(slide.transform, false);
        
        RectTransform imageRect = imageObj.AddComponent<RectTransform>();
        imageRect.anchorMin = new Vector2(0.5f, 0.5f);
        imageRect.anchorMax = new Vector2(0.5f, 0.5f);
        imageRect.pivot = new Vector2(0.5f, 0.5f);
        imageRect.anchoredPosition = new Vector2(0, 150);
        imageRect.sizeDelta = new Vector2(600, 600);
        
        Image image = imageObj.AddComponent<Image>();
        image.color = new Color(0.8f, 0.8f, 0.8f, 0.3f); // Light gray placeholder
        
        // Create text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(slide.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = new Vector2(0, -300);
        textRect.sizeDelta = new Vector2(700, 150);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "Collect the letter to form word";
        text.font = font != null ? font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 48;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = new Color(0, 0, 0, 0.7f);
        outline.effectDistance = new Vector2(2, -2);
        
        // Create Next button
        CreateButton("NextButton", "Next", new Vector2(0, -480), slide.transform, font);
        
        // Hide by default
        slide.SetActive(false);
    }
    
    private static void CreateSlide3(Transform parent, Font font)
    {
        GameObject slide = new GameObject("Slide3");
        slide.transform.SetParent(parent, false);
        
        RectTransform slideRect = slide.AddComponent<RectTransform>();
        slideRect.anchorMin = Vector2.zero;
        slideRect.anchorMax = Vector2.one;
        slideRect.offsetMin = Vector2.zero;
        slideRect.offsetMax = Vector2.zero;
        
        // Create image placeholder
        GameObject imageObj = new GameObject("Image");
        imageObj.transform.SetParent(slide.transform, false);
        
        RectTransform imageRect = imageObj.AddComponent<RectTransform>();
        imageRect.anchorMin = new Vector2(0.5f, 0.5f);
        imageRect.anchorMax = new Vector2(0.5f, 0.5f);
        imageRect.pivot = new Vector2(0.5f, 0.5f);
        imageRect.anchoredPosition = new Vector2(0, 150);
        imageRect.sizeDelta = new Vector2(600, 600);
        
        Image image = imageObj.AddComponent<Image>();
        image.color = new Color(0.8f, 0.8f, 0.8f, 0.3f); // Light gray placeholder
        
        // Create text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(slide.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = new Vector2(0, -300);
        textRect.sizeDelta = new Vector2(700, 150);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "Complete 3 word to win";
        text.font = font != null ? font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 48;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = new Color(0, 0, 0, 0.7f);
        outline.effectDistance = new Vector2(2, -2);
        
        // Create OK button
        CreateButton("OKButton", "OK", new Vector2(0, -480), slide.transform, font);
        
        // Hide by default
        slide.SetActive(false);
    }
    
    private static GameObject CreateButton(string name, string text, Vector2 position, Transform parent, Font font)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);
        
        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(300, 80);
        
        Image image = buttonObj.AddComponent<Image>();
        Sprite buttonSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Sprite/Component/Button_01.png");
        if (buttonSprite != null)
        {
            image.sprite = buttonSprite;
            image.type = Image.Type.Sliced;
        }
        else
        {
            image.color = new Color(0.3f, 0.5f, 0.8f, 1f);
        }
        
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
        buttonText.fontSize = 36;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;
        
        Outline textOutline = textObj.AddComponent<Outline>();
        textOutline.effectColor = new Color(0, 0, 0, 0.7f);
        textOutline.effectDistance = new Vector2(2, -2);
        
        return buttonObj;
    }
}
