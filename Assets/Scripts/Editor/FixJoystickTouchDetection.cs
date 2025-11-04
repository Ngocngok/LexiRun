using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class FixJoystickTouchDetection : MonoBehaviour
{
    [MenuItem("LexiRun/Fix Joystick Touch Detection")]
    public static void Fix()
    {
        // Find the joystick components
        GameObject joystickPanel = GameObject.Find("Canvas/JoystickPanel");
        GameObject joystickBg = GameObject.Find("Canvas/JoystickPanel/JoystickBackground");
        GameObject joystickHandle = GameObject.Find("Canvas/JoystickPanel/JoystickBackground/JoystickHandle");
        
        if (joystickPanel == null || joystickBg == null)
        {
            Debug.LogError("Joystick objects not found!");
            return;
        }
        
        // Remove VirtualJoystick from background if it exists
        VirtualJoystick oldJoystick = joystickBg.GetComponent<VirtualJoystick>();
        if (oldJoystick != null)
        {
            DestroyImmediate(oldJoystick);
        }
        
        // Add VirtualJoystick to the panel (full screen)
        VirtualJoystick joystick = joystickPanel.GetComponent<VirtualJoystick>();
        if (joystick == null)
        {
            joystick = joystickPanel.AddComponent<VirtualJoystick>();
        }
        
        // Get CanvasGroup from background
        CanvasGroup canvasGroup = joystickBg.GetComponent<CanvasGroup>();
        
        // Assign references using SerializedObject
        SerializedObject so = new SerializedObject(joystick);
        
        so.FindProperty("joystickBackground").objectReferenceValue = joystickBg.GetComponent<RectTransform>();
        
        if (joystickHandle != null)
            so.FindProperty("joystickHandle").objectReferenceValue = joystickHandle.GetComponent<RectTransform>();
        
        if (canvasGroup != null)
            so.FindProperty("canvasGroup").objectReferenceValue = canvasGroup;
        
        so.FindProperty("handleRange").floatValue = 50f;
        
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(joystick);
        
        // Make sure panel has Image component for raycast
        Image panelImage = joystickPanel.GetComponent<Image>();
        if (panelImage == null)
        {
            panelImage = joystickPanel.AddComponent<Image>();
        }
        panelImage.color = new Color(1, 1, 1, 0); // Transparent
        panelImage.raycastTarget = true;
        EditorUtility.SetDirty(panelImage);
        
        // Make sure background Image does NOT block raycasts
        Image bgImage = joystickBg.GetComponent<Image>();
        if (bgImage != null)
        {
            bgImage.raycastTarget = false; // Don't block touches
            EditorUtility.SetDirty(bgImage);
        }
        
        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        
        Debug.Log("Joystick touch detection fixed! Now touch anywhere on screen to activate joystick.");
    }
}
