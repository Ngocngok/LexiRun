using UnityEngine;
using UnityEngine.UI;

public class HomeTutorialManager : MonoBehaviour
{
    [Header("Tutorial UI")]
    public GameObject tutorialPanel;
    public GameObject tutorialSlide1;
    public GameObject tutorialSlide2;
    public GameObject tutorialSlide3;
    public GameObject tutorialSlide4;
    public Image tutorialImage1;
    public Image tutorialImage2;
    public Image tutorialImage3;
    public Image tutorialImage4;
    public Button tutorialNextButton1;
    public Button tutorialNextButton2;
    public Button tutorialNextButton3;
    public Button tutorialOKButton;
    public Button openTutorialButton;
    
    private int currentSlide = 1;
    
    void Start()
    {
        // Setup button listeners
        if (openTutorialButton != null)
        {
            openTutorialButton.onClick.AddListener(OpenTutorial);
        }
        
        if (tutorialNextButton1 != null)
        {
            tutorialNextButton1.onClick.AddListener(OnNextSlide1);
        }
        
        if (tutorialNextButton2 != null)
        {
            tutorialNextButton2.onClick.AddListener(OnNextSlide2);
        }
        
        if (tutorialNextButton3 != null)
        {
            tutorialNextButton3.onClick.AddListener(OnNextSlide3);
        }
        
        if (tutorialOKButton != null)
        {
            tutorialOKButton.onClick.AddListener(CloseTutorial);
        }
        
        // Hide tutorial panel initially
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }
    
    public void OpenTutorial()
    {
        if (tutorialPanel != null)
        {
            currentSlide = 1;
            tutorialPanel.SetActive(true);
            ShowSlide(1);
        }
        
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    void ShowSlide(int slideNumber)
    {
        // Hide all slides
        if (tutorialSlide1 != null) tutorialSlide1.SetActive(false);
        if (tutorialSlide2 != null) tutorialSlide2.SetActive(false);
        if (tutorialSlide3 != null) tutorialSlide3.SetActive(false);
        if (tutorialSlide4 != null) tutorialSlide4.SetActive(false);
        
        // Show requested slide
        switch (slideNumber)
        {
            case 1:
                if (tutorialSlide1 != null) tutorialSlide1.SetActive(true);
                break;
            case 2:
                if (tutorialSlide2 != null) tutorialSlide2.SetActive(true);
                break;
            case 3:
                if (tutorialSlide3 != null) tutorialSlide3.SetActive(true);
                break;
            case 4:
                if (tutorialSlide4 != null) tutorialSlide4.SetActive(true);
                break;
        }
        
        currentSlide = slideNumber;
    }
    
    void OnNextSlide1()
    {
        ShowSlide(2);
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    void OnNextSlide2()
    {
        ShowSlide(3);
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    void OnNextSlide3()
    {
        ShowSlide(4);
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    void CloseTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    void OnDestroy()
    {
        if (openTutorialButton != null)
        {
            openTutorialButton.onClick.RemoveListener(OpenTutorial);
        }
        
        if (tutorialNextButton1 != null)
        {
            tutorialNextButton1.onClick.RemoveListener(OnNextSlide1);
        }
        
        if (tutorialNextButton2 != null)
        {
            tutorialNextButton2.onClick.RemoveListener(OnNextSlide2);
        }
        
        if (tutorialNextButton3 != null)
        {
            tutorialNextButton3.onClick.RemoveListener(OnNextSlide3);
        }
        
        if (tutorialOKButton != null)
        {
            tutorialOKButton.onClick.RemoveListener(CloseTutorial);
        }
    }
}
