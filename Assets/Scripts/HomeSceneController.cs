using UnityEngine;
using UnityEngine.UI;

public class HomeSceneController : MonoBehaviour
{
    public Text levelButtonText;
    public Button playButton;
    public Button settingsButton;
    public GameObject settingsPanel;
    public GameObject settingsOverlay;
    
    private int currentLevel;
    
    void Start()
    {
        // Ensure Time.timeScale is reset (in case it was paused in previous scene)
        Time.timeScale = 1f;
        
        currentLevel = SettingsManager.GetCurrentLevel();
        
        if (levelButtonText != null)
        {
            levelButtonText.text = "Level " + currentLevel;
        }
        
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayClicked);
        }
        
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsClicked);
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        
        if (settingsOverlay != null)
        {
            settingsOverlay.SetActive(false);
        }
        
        // Play menu music
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
        }
    }
    
    void OnPlayClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadGameplayScene(currentLevel);
        }
    }
    
    void OnSettingsClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        
        if (settingsOverlay != null)
        {
            settingsOverlay.SetActive(true);
        }
    }
    
    public void CloseSettings()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        
        if (settingsOverlay != null)
        {
            settingsOverlay.SetActive(false);
        }
    }
}
