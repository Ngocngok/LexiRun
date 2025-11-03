using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle sfxToggle;
    public Toggle vibrationToggle;
    public Button closeButton;
    
    public Image musicOnImage;
    public Image musicOffImage;
    public Image sfxOnImage;
    public Image sfxOffImage;
    public Image vibrationOnImage;
    public Image vibrationOffImage;
    
    void Start()
    {
        // Load current settings
        bool musicEnabled = SettingsManager.GetMusicEnabled();
        bool sfxEnabled = SettingsManager.GetSFXEnabled();
        bool vibrationEnabled = SettingsManager.GetVibrationEnabled();
        
        if (musicToggle != null)
        {
            musicToggle.isOn = musicEnabled;
            musicToggle.onValueChanged.AddListener(OnMusicToggled);
            UpdateMusicVisuals(musicEnabled);
        }
        
        if (sfxToggle != null)
        {
            sfxToggle.isOn = sfxEnabled;
            sfxToggle.onValueChanged.AddListener(OnSFXToggled);
            UpdateSFXVisuals(sfxEnabled);
        }
        
        if (vibrationToggle != null)
        {
            vibrationToggle.isOn = vibrationEnabled;
            vibrationToggle.onValueChanged.AddListener(OnVibrationToggled);
            UpdateVibrationVisuals(vibrationEnabled);
        }
        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseClicked);
        }
    }
    
    void OnMusicToggled(bool enabled)
    {
        SettingsManager.SetMusicEnabled(enabled);
        UpdateMusicVisuals(enabled);
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicEnabled(enabled);
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    void OnSFXToggled(bool enabled)
    {
        SettingsManager.SetSFXEnabled(enabled);
        UpdateSFXVisuals(enabled);
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXEnabled(enabled);
            AudioManager.Instance.PlayButtonClick();
        }
    }
    
    void OnVibrationToggled(bool enabled)
    {
        SettingsManager.SetVibrationEnabled(enabled);
        UpdateVibrationVisuals(enabled);
        
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
    
    void UpdateMusicVisuals(bool enabled)
    {
        if (musicOnImage != null) musicOnImage.gameObject.SetActive(enabled);
        if (musicOffImage != null) musicOffImage.gameObject.SetActive(!enabled);
    }
    
    void UpdateSFXVisuals(bool enabled)
    {
        if (sfxOnImage != null) sfxOnImage.gameObject.SetActive(enabled);
        if (sfxOffImage != null) sfxOffImage.gameObject.SetActive(!enabled);
    }
    
    void UpdateVibrationVisuals(bool enabled)
    {
        if (vibrationOnImage != null) vibrationOnImage.gameObject.SetActive(enabled);
        if (vibrationOffImage != null) vibrationOffImage.gameObject.SetActive(!enabled);
    }
}
