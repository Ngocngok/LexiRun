using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateHomeTutorialUI : MonoBehaviour
{
    public static string Execute()
    {
        // Find Canvas
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            return "Canvas not found!";
        }
        
        // Create TutorialPanel
        GameObject tutorialPanel = new GameObject("TutorialPanel");
        tutorialPanel.transform.SetParent(canvas.transform, false);
        
        RectTransform panelRT = tutorialPanel.AddComponent<RectTransform>();
        panelRT.anchorMin = Vector2.zero;
        panelRT.anchorMax = Vector2.one;
        panelRT.sizeDelta = Vector2.zero;
        panelRT.anchoredPosition = Vector2.zero;
        
        Image panelImage = tutorialPanel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.9f);
        
        tutorialPanel.SetActive(false);
        
        // Create 4 tutorial slides
        GameObject[] slides = new GameObject[4];
        Image[] images = new Image[4];
        Button[] nextButtons = new Button[4];
        
        string[] tutorialTextures = { "tut1", "tut2", "tut3", "tut4" };
        
        for (int i = 0; i < 4; i++)
        {
            // Create slide
            GameObject slide = new GameObject("TutorialSlide" + (i + 1));
            slide.transform.SetParent(tutorialPanel.transform, false);
            
            RectTransform slideRT = slide.AddComponent<RectTransform>();
            slideRT.anchorMin = Vector2.zero;
            slideRT.anchorMax = Vector2.one;
            slideRT.sizeDelta = Vector2.zero;
            slideRT.anchoredPosition = Vector2.zero;
            
            slide.SetActive(false);
            
            // Create image
            GameObject imageObj = new GameObject("TutorialImage");
            imageObj.transform.SetParent(slide.transform, false);
            
            RectTransform imageRT = imageObj.AddComponent<RectTransform>();
            imageRT.anchorMin = new Vector2(0.5f, 0.5f);
            imageRT.anchorMax = new Vector2(0.5f, 0.5f);
            imageRT.sizeDelta = new Vector2(800, 800);
            imageRT.anchoredPosition = Vector2.zero;
            
            Image image = imageObj.AddComponent<Image>();
            image.color = Color.white;
            
            // Try to load texture
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/" + tutorialTextures[i] + ".png");
            if (texture == null)
            {
                texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/" + tutorialTextures[i] + ".jpg");
            }
            
            if (texture != null)
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                image.sprite = sprite;
            }
            
            images[i] = image;
            
            // Create button
            GameObject buttonObj = new GameObject(i < 3 ? "NextButton" + (i + 1) : "OKButton");
            buttonObj.transform.SetParent(slide.transform, false);
            
            RectTransform buttonRT = buttonObj.AddComponent<RectTransform>();
            buttonRT.anchorMin = new Vector2(0.5f, 0.0f);
            buttonRT.anchorMax = new Vector2(0.5f, 0.0f);
            buttonRT.sizeDelta = new Vector2(200, 80);
            buttonRT.anchoredPosition = new Vector2(0, 100);
            
            Button button = buttonObj.AddComponent<Button>();
            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.6f, 1f, 1f);
            
            // Add button text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            
            RectTransform textRT = textObj.AddComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.sizeDelta = Vector2.zero;
            textRT.anchoredPosition = Vector2.zero;
            
            Text text = textObj.AddComponent<Text>();
            text.text = i < 3 ? "Next" : "OK";
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 30;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            
            nextButtons[i] = button;
            slides[i] = slide;
        }
        
        // Find HomeTutorialManager and assign references
        HomeTutorialManager manager = GameObject.FindFirstObjectByType<HomeTutorialManager>();
        if (manager != null)
        {
            manager.tutorialPanel = tutorialPanel;
            manager.tutorialSlide1 = slides[0];
            manager.tutorialSlide2 = slides[1];
            manager.tutorialSlide3 = slides[2];
            manager.tutorialSlide4 = slides[3];
            manager.tutorialImage1 = images[0];
            manager.tutorialImage2 = images[1];
            manager.tutorialImage3 = images[2];
            manager.tutorialImage4 = images[3];
            manager.tutorialNextButton1 = nextButtons[0];
            manager.tutorialNextButton2 = nextButtons[1];
            manager.tutorialNextButton3 = nextButtons[2];
            manager.tutorialOKButton = nextButtons[3];
            
            EditorUtility.SetDirty(manager);
        }
        
        // Mark scene as dirty and save
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        
        return "Successfully created tutorial UI in HomeScene with 4 slides!";
    }
}
