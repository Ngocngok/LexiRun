using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    public Button musicButton;
    public Button sfxButton;
    public Button vibrationButton;
    public Button closeButton;
    
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;
    public Sprite vibrationOnSprite;
    public Sprite vibrationOffSprite;
    
    private bool musicEnabled;
    private bool sfxEnabled;
    private bool vibrationEnabled;
    
    void Start()
    {
        // Load current settings
        musicEnabled = SettingsManager.GetMusicEnabled();
        sfxEnabled = SettingsManager.GetSFXEnabled();
        vibrationEnabled = SettingsManager.GetVibrationEnabled();
        
        // Setup button listeners
        if (musicButton != null)
        {
            musicButton.onClick.AddListener(OnMusicButtonClicked);
            UpdateMusicVisuals();
        }
        
        if (sfxButton != null)
        {
            sfxButton.onClick.AddListener(OnSFXButtonClicked);
            UpdateSFXVisuals();
        }
        
        if (vibrationButton != null)
        {
            vibrationButton.onClick.AddListener(OnVibrationButtonClicked);
            UpdateVibrationVisuals();
        }
        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseClicked);
        }
    }
    
    void OnMusicButtonClicked()
    {
        musicEnabled = !musicEnabled;
        SettingsManager.SetMusicEnabled(musicEnabled);
        UpdateMusicVisuals();
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicEnabled(musicEnabled);
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    void OnSFXButtonClicked()
    {
        sfxEnabled = !sfxEnabled;
        SettingsManager.SetSFXEnabled(sfxEnabled);
        UpdateSFXVisuals();
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXEnabled(sfxEnabled);
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    void OnVibrationButtonClicked()
    {
        vibrationEnabled = !vibrationEnabled;
        SettingsManager.SetVibrationEnabled(vibrationEnabled);
        UpdateVibrationVisuals();
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    void OnCloseClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        HomeSceneController homeController = FindFirstObjectByType<HomeSceneController>();
        if (homeController != null)
        {
            homeController.CloseSettings();
        }
    }
    
    void UpdateMusicVisuals()
    {
        if (musicButton != null)
        {
            Image buttonImage = musicButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = musicEnabled ? musicOnSprite : musicOffSprite;
            }
        }
    }
    
    void UpdateSFXVisuals()
    {
        if (sfxButton != null)
        {
            Image buttonImage = sfxButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = sfxEnabled ? sfxOnSprite : sfxOffSprite;
            }
        }
    }
    
    void UpdateVibrationVisuals()
    {
        if (vibrationButton != null)
        {
            Image buttonImage = vibrationButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = vibrationEnabled ? vibrationOnSprite : vibrationOffSprite;
            }
        }
    }
}
