using UnityEngine;
using UnityEditor;

public class ShowPausePanel : MonoBehaviour
{
    [MenuItem("LexiRun/Toggle Pause Panel Visibility")]
    public static void Toggle()
    {
        GameObject pauseOverlay = GameObject.Find("Canvas/PauseOverlay");
        if (pauseOverlay != null)
        {
            pauseOverlay.SetActive(!pauseOverlay.activeSelf);
            Debug.Log("Pause panel visibility toggled: " + pauseOverlay.activeSelf);
        }
        else
        {
            Debug.LogError("Pause overlay not found!");
        }
    }
}
