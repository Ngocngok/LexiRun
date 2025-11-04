using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AssignSettingsButtons : MonoBehaviour
{
    [MenuItem("LexiRun/Assign Settings Button References")]
    public static void AssignReferences()
    {
        // Find the settings panel
        GameObject settingsPanel = GameObject.Find("Canvas/SettingsOverlay/SettingsPanel");
        if (settingsPanel == null)
        {
            Debug.LogError("Settings panel not found!");
            return;
        }
        
        SettingsPanelController controller = settingsPanel.GetComponent<SettingsPanelController>();
        if (controller == null)
        {
            Debug.LogError("SettingsPanelController not found!");
            return;
        }
        
        // Find buttons
        GameObject musicButton = GameObject.Find("Canvas/SettingsOverlay/SettingsPanel/ButtonsContainer/MusicButtonContainer/MusicButton");
        GameObject soundButton = GameObject.Find("Canvas/SettingsOverlay/SettingsPanel/ButtonsContainer/SoundButtonContainer/SoundButton");
        GameObject hapticsButton = GameObject.Find("Canvas/SettingsOverlay/SettingsPanel/ButtonsContainer/HapticsButtonContainer/HapticsButton");
        GameObject closeButton = GameObject.Find("Canvas/SettingsOverlay/SettingsPanel/CloseButton");
        
        // Assign using SerializedObject
        SerializedObject so = new SerializedObject(controller);
        
        if (musicButton != null)
            so.FindProperty("musicButton").objectReferenceValue = musicButton.GetComponent<Button>();
        
        if (soundButton != null)
            so.FindProperty("sfxButton").objectReferenceValue = soundButton.GetComponent<Button>();
        
        if (hapticsButton != null)
            so.FindProperty("vibrationButton").objectReferenceValue = hapticsButton.GetComponent<Button>();
        
        if (closeButton != null)
            so.FindProperty("closeButton").objectReferenceValue = closeButton.GetComponent<Button>();
        
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(controller);
        
        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        
        Debug.Log("Settings button references assigned successfully!");
    }
}
