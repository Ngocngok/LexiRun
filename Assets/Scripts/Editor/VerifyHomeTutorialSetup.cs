using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class VerifyHomeTutorialSetup : MonoBehaviour
{
    public static string Execute()
    {
        // Find HomeTutorialManager
        HomeTutorialManager manager = GameObject.FindFirstObjectByType<HomeTutorialManager>();
        if (manager == null)
        {
            return "HomeTutorialManager not found!";
        }
        
        // Find TutorialPanel
        GameObject tutorialPanel = GameObject.Find("TutorialPanel");
        if (tutorialPanel == null)
        {
            // Try to find it under Canvas
            Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                Transform found = canvas.transform.Find("TutorialPanel");
                if (found != null)
                {
                    tutorialPanel = found.gameObject;
                }
            }
        }
        
        if (tutorialPanel == null)
        {
            return "TutorialPanel not found in scene!";
        }
        
        // Assign tutorial panel
        manager.tutorialPanel = tutorialPanel;
        
        // Find and assign slides
        for (int i = 1; i <= 4; i++)
        {
            Transform slideTransform = tutorialPanel.transform.Find("TutorialSlide" + i);
            if (slideTransform != null)
            {
                GameObject slide = slideTransform.gameObject;
                Image image = slideTransform.Find("TutorialImage")?.GetComponent<Image>();
                Button button = slideTransform.GetComponentInChildren<Button>();
                
                switch (i)
                {
                    case 1:
                        manager.tutorialSlide1 = slide;
                        manager.tutorialImage1 = image;
                        manager.tutorialNextButton1 = button;
                        break;
                    case 2:
                        manager.tutorialSlide2 = slide;
                        manager.tutorialImage2 = image;
                        manager.tutorialNextButton2 = button;
                        break;
                    case 3:
                        manager.tutorialSlide3 = slide;
                        manager.tutorialImage3 = image;
                        manager.tutorialNextButton3 = button;
                        break;
                    case 4:
                        manager.tutorialSlide4 = slide;
                        manager.tutorialImage4 = image;
                        manager.tutorialOKButton = button;
                        break;
                }
            }
        }
        
        // Find and assign tutorial button
        Button tutorialButton = GameObject.Find("TutorialButton")?.GetComponent<Button>();
        if (tutorialButton != null)
        {
            manager.openTutorialButton = tutorialButton;
        }
        
        EditorUtility.SetDirty(manager);
        
        return "HomeTutorialManager setup complete!\n" +
               "Panel: " + (manager.tutorialPanel != null) + "\n" +
               "Slide1: " + (manager.tutorialSlide1 != null) + "\n" +
               "Slide2: " + (manager.tutorialSlide2 != null) + "\n" +
               "Slide3: " + (manager.tutorialSlide3 != null) + "\n" +
               "Slide4: " + (manager.tutorialSlide4 != null) + "\n" +
               "Button: " + (manager.openTutorialButton != null);
    }
}
