using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AssignPauseReferences : MonoBehaviour
{
    [MenuItem("LexiRun/Assign Pause References")]
    public static void Assign()
    {
        // Find UIManager
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found!");
            return;
        }
        
        // Find pause UI elements
        GameObject pauseButton = GameObject.Find("Canvas/PauseButton");
        GameObject pauseOverlay = GameObject.Find("Canvas/PauseOverlay");
        GameObject resumeButton = GameObject.Find("Canvas/PauseOverlay/PausePanel/ButtonsContainer/ResumeButton");
        GameObject homeButton = GameObject.Find("Canvas/PauseOverlay/PausePanel/ButtonsContainer/HomeButton");
        
        // Assign using SerializedObject
        SerializedObject so = new SerializedObject(uiManager);
        
        if (pauseButton != null)
        {
            so.FindProperty("pauseButton").objectReferenceValue = pauseButton.GetComponent<Button>();
            Debug.Log("Assigned pause button");
        }
        
        if (pauseOverlay != null)
        {
            so.FindProperty("pausePanel").objectReferenceValue = pauseOverlay;
            Debug.Log("Assigned pause panel");
        }
        
        if (resumeButton != null)
        {
            so.FindProperty("pauseResumeButton").objectReferenceValue = resumeButton.GetComponent<Button>();
            Debug.Log("Assigned resume button");
        }
        
        if (homeButton != null)
        {
            so.FindProperty("pauseHomeButton").objectReferenceValue = homeButton.GetComponent<Button>();
            Debug.Log("Assigned home button");
        }
        
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(uiManager);
        
        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        
        Debug.Log("Pause references assigned successfully!");
    }
}
