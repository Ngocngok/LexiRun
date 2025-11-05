using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UpdateGameplayTutorialSlides : MonoBehaviour
{
    public static string Execute()
    {
        // Find UIManager
        UIManager uiManager = GameObject.FindFirstObjectByType<UIManager>();
        if (uiManager == null)
        {
            return "UIManager not found!";
        }
        
        // Get slide 3 (will become slide 4)
        GameObject slide3 = uiManager.tutorialSlide3;
        if (slide3 == null)
        {
            return "TutorialSlide3 not found!";
        }
        
        // Duplicate slide 3 to create slide 4
        GameObject slide4 = GameObject.Instantiate(slide3, slide3.transform.parent);
        slide4.name = "TutorialSlide4";
        slide4.SetActive(false);
        
        // Rename slide 3's button to NextButton3
        Button slide3Button = slide3.GetComponentInChildren<Button>();
        if (slide3Button != null)
        {
            slide3Button.gameObject.name = "NextButton3";
            Text buttonText = slide3Button.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Next";
            }
        }
        
        // Update slide 3 image to use tut3
        Image slide3Image = slide3.transform.Find("TutorialImage")?.GetComponent<Image>();
        if (slide3Image != null)
        {
            Texture2D tut3Texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/tut3.png");
            if (tut3Texture != null)
            {
                Sprite sprite = Sprite.Create(tut3Texture, new Rect(0, 0, tut3Texture.width, tut3Texture.height), new Vector2(0.5f, 0.5f));
                slide3Image.sprite = sprite;
            }
        }
        
        // Update slide 4 button to OKButton
        Button slide4Button = slide4.GetComponentInChildren<Button>();
        if (slide4Button != null)
        {
            slide4Button.gameObject.name = "OKButton";
            Text buttonText = slide4Button.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "OK";
            }
        }
        
        // Assign to UIManager
        uiManager.tutorialSlide4 = slide4;
        
        Image slide4Image = slide4.transform.Find("TutorialImage")?.GetComponent<Image>();
        if (slide4Image != null)
        {
            uiManager.tutorialImage4 = slide4Image;
        }
        
        uiManager.tutorialImage3 = slide3Image;
        uiManager.tutorialNextButton3 = slide3Button;
        uiManager.tutorialOKButton = slide4Button;
        
        EditorUtility.SetDirty(uiManager);
        
        return "Successfully updated GameplayScene tutorial to 4 slides!";
    }
}
