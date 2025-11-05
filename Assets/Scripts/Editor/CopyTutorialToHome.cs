using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;

public class CopyTutorialToHome : MonoBehaviour
{
    public static string Execute()
    {
        // Save current scene
        string currentScenePath = SceneManager.GetActiveScene().path;
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        
        // Open GameplayScene
        Scene gameplayScene = EditorSceneManager.OpenScene("Assets/Scenes/GameplayScene.unity", OpenSceneMode.Single);
        
        // Find tutorial panel - search in all root objects
        GameObject tutorialPanel = null;
        foreach (GameObject obj in gameplayScene.GetRootGameObjects())
        {
            Transform found = obj.transform.Find("Canvas/TutorialOverlay");
            if (found != null)
            {
                tutorialPanel = found.gameObject;
                break;
            }
            
            // Also try direct search
            if (obj.name == "Canvas")
            {
                found = obj.transform.Find("TutorialOverlay");
                if (found != null)
                {
                    tutorialPanel = found.gameObject;
                    break;
                }
            }
        }
        if (tutorialPanel == null)
        {
            EditorSceneManager.OpenScene(currentScenePath, OpenSceneMode.Single);
            return "TutorialOverlay not found in GameplayScene!";
        }
        
        // Create a prefab from the tutorial panel
        string prefabPath = "Assets/Prefabs/TutorialPanelTemp.prefab";
        PrefabUtility.SaveAsPrefabAsset(tutorialPanel, prefabPath);
        
        // Open HomeScene
        EditorSceneManager.OpenScene(currentScenePath, OpenSceneMode.Single);
        
        // Load the prefab
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        
        // Find Canvas
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            return "Canvas not found in HomeScene!";
        }
        
        // Instantiate the tutorial panel
        GameObject newTutorialPanel = PrefabUtility.InstantiatePrefab(prefab, canvas.transform) as GameObject;
        newTutorialPanel.name = "TutorialPanel";
        newTutorialPanel.SetActive(false);
        
        // Find HomeTutorialManager
        HomeTutorialManager manager = GameObject.FindFirstObjectByType<HomeTutorialManager>();
        if (manager != null)
        {
            manager.tutorialPanel = newTutorialPanel;
            
            // Find and assign all references
            Transform slide1 = newTutorialPanel.transform.Find("TutorialSlide1");
            Transform slide2 = newTutorialPanel.transform.Find("TutorialSlide2");
            Transform slide3 = newTutorialPanel.transform.Find("TutorialSlide3");
            Transform slide4 = newTutorialPanel.transform.Find("TutorialSlide4");
            
            if (slide1 != null)
            {
                manager.tutorialSlide1 = slide1.gameObject;
                manager.tutorialImage1 = slide1.Find("TutorialImage")?.GetComponent<Image>();
                manager.tutorialNextButton1 = slide1.GetComponentInChildren<Button>();
            }
            
            if (slide2 != null)
            {
                manager.tutorialSlide2 = slide2.gameObject;
                manager.tutorialImage2 = slide2.Find("TutorialImage")?.GetComponent<Image>();
                manager.tutorialNextButton2 = slide2.GetComponentInChildren<Button>();
            }
            
            if (slide3 != null)
            {
                manager.tutorialSlide3 = slide3.gameObject;
                manager.tutorialImage3 = slide3.Find("TutorialImage")?.GetComponent<Image>();
                
                // Find NextButton3
                Button[] buttons = slide3.GetComponentsInChildren<Button>(true);
                foreach (Button btn in buttons)
                {
                    if (btn.name == "NextButton3" || btn.name.Contains("Next"))
                    {
                        manager.tutorialNextButton3 = btn;
                        break;
                    }
                }
            }
            
            if (slide4 != null)
            {
                manager.tutorialSlide4 = slide4.gameObject;
                manager.tutorialImage4 = slide4.Find("TutorialImage")?.GetComponent<Image>();
                
                // Find OKButton
                Button[] buttons = slide4.GetComponentsInChildren<Button>(true);
                foreach (Button btn in buttons)
                {
                    if (btn.name == "OKButton" || btn.name.Contains("OK"))
                    {
                        manager.tutorialOKButton = btn;
                        break;
                    }
                }
            }
            
            EditorUtility.SetDirty(manager);
        }
        
        // Delete temp prefab
        AssetDatabase.DeleteAsset(prefabPath);
        
        // Save scene
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        
        return "Successfully copied tutorial panel to HomeScene!";
    }
}
