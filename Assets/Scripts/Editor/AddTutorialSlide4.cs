using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddTutorialSlide4 : MonoBehaviour
{
    public static string Execute()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        
        // Find UIManager
        UIManager uiManager = GameObject.FindFirstObjectByType<UIManager>();
        if (uiManager == null)
        {
            return "UIManager not found!";
        }
        
        // Get tutorialSlide3
        GameObject tutorialSlide3 = uiManager.tutorialSlide3;
        if (tutorialSlide3 == null)
        {
            return "TutorialSlide3 not found!";
        }
        
        // Duplicate tutorialSlide3 to create tutorialSlide4
        GameObject tutorialSlide4 = GameObject.Instantiate(tutorialSlide3, tutorialSlide3.transform.parent);
        tutorialSlide4.name = "TutorialSlide4";
        tutorialSlide4.SetActive(false);
        
        // Update the image reference
        Image image4 = tutorialSlide4.transform.Find("TutorialImage")?.GetComponent<Image>();
        
        // Load tut3 texture
        Texture2D tut3Texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/tut3.png");
        if (tut3Texture == null)
        {
            tut3Texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/tut3.jpg");
        }
        
        if (tut3Texture != null && image4 != null)
        {
            // Create sprite from texture
            Sprite sprite = Sprite.Create(tut3Texture, new Rect(0, 0, tut3Texture.width, tut3Texture.height), new Vector2(0.5f, 0.5f));
            image4.sprite = sprite;
        }
        
        // Find the Next button and rename it
        Button nextButton3 = tutorialSlide3.GetComponentInChildren<Button>();
        if (nextButton3 != null)
        {
            nextButton3.gameObject.name = "NextButton3";
        }
        
        // Find the OK button in slide4 and keep it as OK
        Button okButton = tutorialSlide4.GetComponentInChildren<Button>();
        if (okButton != null)
        {
            okButton.gameObject.name = "OKButton";
        }
        
        // Assign references to UIManager
        uiManager.tutorialSlide4 = tutorialSlide4;
        uiManager.tutorialImage4 = image4;
        uiManager.tutorialNextButton3 = nextButton3;
        uiManager.tutorialOKButton = okButton;
        
        EditorUtility.SetDirty(uiManager);
        
        return "Successfully created TutorialSlide4!";
    }
}
