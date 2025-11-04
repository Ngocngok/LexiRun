using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class RebuildSettingsPanel : MonoBehaviour
{
    [MenuItem("LexiRun/Rebuild Settings Panel")]
    public static void Rebuild()
    {
        // Find the settings panel
        GameObject settingsPanel = GameObject.Find("Canvas/SettingsOverlay/SettingsPanel");
        if (settingsPanel == null)
        {
            Debug.LogError("Settings panel not found!");
            return;
        }
        
        // Load sprites
        Sprite musicOn = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Sprite/Demo/Demo_Icon/Icon_PictoIcon_Music_on.png");
        Sprite musicOff = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Sprite/Demo/Demo_Icon/Icon_PictoIcon_Music_off.png");
        Sprite soundOn = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Sprite/Demo/Demo_Icon/Icon_PictoIcon_Sound_on.png");
        Sprite soundOff = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Sprite/Demo/Demo_Icon/Icon_PictoIcon_Sound_off.png");
        Sprite hapticOn = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Sprite/Demo/Demo_Icon/Icon_PictoIcon_Haptic.png");
        Sprite hapticOff = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI PRO Kit - Casual Game/ResourcesData/Sprite/Demo/Demo_Icon/Icon_PictoIcon_Haptic_off.png");
        
        // Delete old toggles
        Transform musicToggle = settingsPanel.transform.Find("MusicToggle");
        Transform sfxToggle = settingsPanel.transform.Find("SFXToggle");
        Transform vibrationToggle = settingsPanel.transform.Find("VibrationToggle");
        
        if (musicToggle != null) DestroyImmediate(musicToggle.gameObject);
        if (sfxToggle != null) DestroyImmediate(sfxToggle.gameObject);
        if (vibrationToggle != null) DestroyImmediate(vibrationToggle.gameObject);
        
        // Create buttons container
        GameObject buttonsContainer = new GameObject("ButtonsContainer");
        buttonsContainer.transform.SetParent(settingsPanel.transform, false);
        
        RectTransform containerRect = buttonsContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.anchoredPosition = new Vector2(0, -50);
        containerRect.sizeDelta = new Vector2(400, 150);
        
        // Add horizontal layout
        HorizontalLayoutGroup layout = buttonsContainer.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 40;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        
        // Create Music button
        GameObject musicButton = CreateSettingButton("MusicButton", "Music", musicOn, buttonsContainer.transform);
        
        // Create Sound button
        GameObject soundButton = CreateSettingButton("SoundButton", "Sound", soundOn, buttonsContainer.transform);
        
        // Create Haptics button
        GameObject hapticsButton = CreateSettingButton("HapticsButton", "Haptics", hapticOn, buttonsContainer.transform);
        
        // Update SettingsPanelController
        SettingsPanelController controller = settingsPanel.GetComponent<SettingsPanelController>();
        if (controller != null)
        {
            SerializedObject so = new SerializedObject(controller);
            
            so.FindProperty("musicButton").objectReferenceValue = musicButton.GetComponent<Button>();
            so.FindProperty("sfxButton").objectReferenceValue = soundButton.GetComponent<Button>();
            so.FindProperty("vibrationButton").objectReferenceValue = hapticsButton.GetComponent<Button>();
            
            so.FindProperty("musicOnSprite").objectReferenceValue = musicOn;
            so.FindProperty("musicOffSprite").objectReferenceValue = musicOff;
            so.FindProperty("sfxOnSprite").objectReferenceValue = soundOn;
            so.FindProperty("sfxOffSprite").objectReferenceValue = soundOff;
            so.FindProperty("vibrationOnSprite").objectReferenceValue = hapticOn;
            so.FindProperty("vibrationOffSprite").objectReferenceValue = hapticOff;
            
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(controller);
        }
        
        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        
        Debug.Log("Settings panel rebuilt successfully!");
    }
    
    private static GameObject CreateSettingButton(string name, string label, Sprite sprite, Transform parent)
    {
        // Create container
        GameObject container = new GameObject(name + "Container");
        container.transform.SetParent(parent, false);
        
        RectTransform containerRect = container.AddComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(100, 150);
        
        // Add vertical layout
        VerticalLayoutGroup vertLayout = container.AddComponent<VerticalLayoutGroup>();
        vertLayout.spacing = 10;
        vertLayout.childAlignment = TextAnchor.MiddleCenter;
        vertLayout.childControlWidth = false;
        vertLayout.childControlHeight = false;
        vertLayout.childForceExpandWidth = false;
        vertLayout.childForceExpandHeight = false;
        
        // Create label
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(container.transform, false);
        
        RectTransform labelRect = labelObj.AddComponent<RectTransform>();
        labelRect.sizeDelta = new Vector2(100, 30);
        
        Text labelText = labelObj.AddComponent<Text>();
        labelText.text = label;
        labelText.fontSize = 24;
        labelText.alignment = TextAnchor.MiddleCenter;
        labelText.color = Color.white;
        
        // Apply LilitaOne font
        Font lilitaFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Resources/LilitaOne-Regular.ttf");
        if (lilitaFont != null)
        {
            labelText.font = lilitaFont;
        }
        else
        {
            labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }
        
        // Add outline
        Outline outline = labelObj.AddComponent<Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, 0.7f);
        outline.effectDistance = new Vector2(2f, -2f);
        
        // Create button
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(container.transform, false);
        
        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(80, 80);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.sprite = sprite;
        buttonImage.preserveAspect = true;
        
        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;
        button.transition = Selectable.Transition.ColorTint;
        
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f);
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f);
        colors.selectedColor = Color.white;
        colors.disabledColor = new Color(0.5f, 0.5f, 0.5f);
        button.colors = colors;
        
        return buttonObj;
    }
}
