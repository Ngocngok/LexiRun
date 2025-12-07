using UnityEngine;
using UnityEditor;
using LexiRun.UI;

public class SetupLoadingLetters : MonoBehaviour
{
    public static void Setup()
    {
        string[] letterNames = new string[] { "Letter1", "Letter2", "Letter3", "Letter4" };
        float delayStep = 0.2f;

        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("Canvas not found");
            return;
        }

        Transform loadingPanel = canvas.transform.Find("LoadingPanel");
        if (loadingPanel == null)
        {
            Debug.LogError("LoadingPanel not found");
            return;
        }

        Transform container = loadingPanel.Find("GameObject");
        if (container == null)
        {
            Debug.LogError("GameObject container not found");
            return;
        }

        for (int i = 0; i < letterNames.Length; i++)
        {
            Transform letterTr = container.Find(letterNames[i]);
            if (letterTr != null)
            {
                GameObject obj = letterTr.gameObject;
                UILetterAnimation anim = obj.GetComponent<UILetterAnimation>();
                if (anim == null)
                {
                    anim = obj.AddComponent<UILetterAnimation>();
                }

                SerializedObject so = new SerializedObject(anim);
                so.FindProperty("delay").floatValue = 0f;
                so.FindProperty("minScale").floatValue = 0.9f;
                so.FindProperty("maxScale").floatValue = 1.1f;
                so.FindProperty("scaleSpeed").floatValue = 3f;
                so.FindProperty("rotationAngle").floatValue = 5f;
                so.FindProperty("rotationSpeed").floatValue = 3f;
                so.ApplyModifiedProperties();
                
                Debug.Log($"Setup animation for {obj.name}");
            }
            else
            {
                Debug.LogWarning($"Could not find {letterNames[i]} in {container.name}");
            }
        }
    }
}
