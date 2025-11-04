using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SetupFloatingJoystick : MonoBehaviour
{
    [MenuItem("LexiRun/Setup Floating Joystick")]
    public static void Setup()
    {
        // Find the joystick background
        GameObject joystickBg = GameObject.Find("Canvas/JoystickPanel/JoystickBackground");
        if (joystickBg == null)
        {
            Debug.LogError("Joystick background not found!");
            return;
        }
        
        // Add CanvasGroup if not present
        CanvasGroup canvasGroup = joystickBg.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = joystickBg.AddComponent<CanvasGroup>();
        }
        
        // Set initial alpha to 0 (hidden)
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        
        // Find the VirtualJoystick component
        VirtualJoystick joystick = FindFirstObjectByType<VirtualJoystick>();
        if (joystick != null)
        {
            SerializedObject so = new SerializedObject(joystick);
            so.FindProperty("canvasGroup").objectReferenceValue = canvasGroup;
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(joystick);
        }
        
        // Make the JoystickPanel cover the entire screen for touch detection
        GameObject joystickPanel = GameObject.Find("Canvas/JoystickPanel");
        if (joystickPanel != null)
        {
            RectTransform panelRect = joystickPanel.GetComponent<RectTransform>();
            if (panelRect != null)
            {
                // Stretch to fill entire canvas
                panelRect.anchorMin = Vector2.zero;
                panelRect.anchorMax = Vector2.one;
                panelRect.offsetMin = Vector2.zero;
                panelRect.offsetMax = Vector2.zero;
                
                EditorUtility.SetDirty(joystickPanel);
            }
            
            // Make sure the panel has an Image component for raycast target
            Image panelImage = joystickPanel.GetComponent<Image>();
            if (panelImage == null)
            {
                panelImage = joystickPanel.AddComponent<Image>();
            }
            panelImage.color = new Color(1, 1, 1, 0); // Transparent
            panelImage.raycastTarget = true;
            EditorUtility.SetDirty(panelImage);
        }
        
        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        
        Debug.Log("Floating joystick setup complete!");
    }
}
