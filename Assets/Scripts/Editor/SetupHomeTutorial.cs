using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class SetupHomeTutorial : MonoBehaviour
{
    public static string Execute()
    {
        // Load GameplayScene to get tutorial structure
        Scene gameplayScene = EditorSceneManager.OpenScene("Assets/Scenes/GameplayScene.unity", OpenSceneMode.Additive);
        
        // Find UIManager in gameplay scene
        UIManager gameplayUIManager = null;
        foreach (GameObject obj in gameplayScene.GetRootGameObjects())
        {
            gameplayUIManager = obj.GetComponentInChildren<UIManager>(true);
            if (gameplayUIManager != null) break;
        }
        
        if (gameplayUIManager == null || gameplayUIManager.tutorialPanel == null)
        {
            EditorSceneManager.CloseScene(gameplayScene, true);
            return "Tutorial panel not found in GameplayScene!";
        }
        
        // Get the tutorial panel
        GameObject tutorialPanelSource = gameplayUIManager.tutorialPanel;
        
        // Switch back to HomeScene
        Scene homeScene = SceneManager.GetSceneByPath("Assets/Scenes/HomeScene.unity");
        SceneManager.SetActiveScene(homeScene);
        
        // Find Canvas in HomeScene
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            EditorSceneManager.CloseScene(gameplayScene, true);
            return "Canvas not found in HomeScene!";
        }
        
        // Duplicate the tutorial panel
        GameObject tutorialPanel = GameObject.Instantiate(tutorialPanelSource, canvas.transform);
        tutorialPanel.name = "TutorialPanel";
        tutorialPanel.SetActive(false);
        
        // Create HomeTutorialManager
        GameObject managerObj = new GameObject("HomeTutorialManager");
        HomeTutorialManager manager = managerObj.AddComponent<HomeTutorialManager>();
        
        // Assign references
        manager.tutorialPanel = tutorialPanel;
        
        // Find slides
        Transform slide1 = tutorialPanel.transform.Find("TutorialSlide1");
        Transform slide2 = tutorialPanel.transform.Find("TutorialSlide2");
        Transform slide3 = tutorialPanel.transform.Find("TutorialSlide3");
        Transform slide4 = tutorialPanel.transform.Find("TutorialSlide4");
        
        if (slide1 != null) manager.tutorialSlide1 = slide1.gameObject;
        if (slide2 != null) manager.tutorialSlide2 = slide2.gameObject;
        if (slide3 != null) manager.tutorialSlide3 = slide3.gameObject;
        if (slide4 != null) manager.tutorialSlide4 = slide4.gameObject;
        
        // Find images
        if (slide1 != null) manager.tutorialImage1 = slide1.Find("TutorialImage")?.GetComponent<Image>();
        if (slide2 != null) manager.tutorialImage2 = slide2.Find("TutorialImage")?.GetComponent<Image>();
        if (slide3 != null) manager.tutorialImage3 = slide3.Find("TutorialImage")?.GetComponent<Image>();
        if (slide4 != null) manager.tutorialImage4 = slide4.Find("TutorialImage")?.GetComponent<Image>();
        
        // Find buttons
        if (slide1 != null) manager.tutorialNextButton1 = slide1.GetComponentInChildren<Button>();
        if (slide2 != null) manager.tutorialNextButton2 = slide2.GetComponentInChildren<Button>();
        if (slide3 != null) manager.tutorialNextButton3 = slide3.Find("NextButton3")?.GetComponent<Button>();
        if (slide4 != null) manager.tutorialOKButton = slide4.Find("OKButton")?.GetComponent<Button>();
        
        // Find the tutorial button
        Button tutorialButton = GameObject.Find("TutorialButton")?.GetComponent<Button>();
        if (tutorialButton != null)
        {
            manager.openTutorialButton = tutorialButton;
        }
        
        EditorUtility.SetDirty(manager);
        
        // Close gameplay scene
        EditorSceneManager.CloseScene(gameplayScene, true);
        
        return "Successfully setup home tutorial!";
    }
}
