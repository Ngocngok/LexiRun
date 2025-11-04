using UnityEngine;
using UnityEditor;

public class AssignJoystickReferences : MonoBehaviour
{
    [MenuItem("LexiRun/Assign Joystick References")]
    public static void Assign()
    {
        // Find components
        GameObject joystickBg = GameObject.Find("Canvas/JoystickPanel/JoystickBackground");
        GameObject joystickHandle = GameObject.Find("Canvas/JoystickPanel/JoystickBackground/JoystickHandle");
        
        if (joystickBg == null)
        {
            Debug.LogError("Joystick background not found!");
            return;
        }
        
        VirtualJoystick joystick = joystickBg.GetComponent<VirtualJoystick>();
        if (joystick == null)
        {
            Debug.LogError("VirtualJoystick component not found!");
            return;
        }
        
        CanvasGroup canvasGroup = joystickBg.GetComponent<CanvasGroup>();
        
        // Assign using SerializedObject
        SerializedObject so = new SerializedObject(joystick);
        
        so.FindProperty("joystickBackground").objectReferenceValue = joystickBg.GetComponent<RectTransform>();
        
        if (joystickHandle != null)
            so.FindProperty("joystickHandle").objectReferenceValue = joystickHandle.GetComponent<RectTransform>();
        
        if (canvasGroup != null)
            so.FindProperty("canvasGroup").objectReferenceValue = canvasGroup;
        
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(joystick);
        
        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        
        Debug.Log("Joystick references assigned successfully!");
    }
}
