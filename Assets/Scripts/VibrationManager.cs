using UnityEngine;
using Lofelt.NiceVibrations;
using MoreMountains.NiceVibrations;

/// <summary>
/// Manages haptic feedback/vibration for the game using the Feel package (Nice Vibrations).
/// Respects the VibrationEnabled setting from SettingsManager.
/// </summary>
public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance { get; private set; }
    
    [Header("Vibration Settings")]
    [Tooltip("Enable/disable vibration globally (overrides settings)")]
    [SerializeField] private bool enableVibration = true;
    
    [Header("Haptic Presets")]
    [Tooltip("Preset for correct letter collection (subtle)")]
    [SerializeField] private HapticPatterns.PresetType correctLetterPreset = HapticPatterns.PresetType.LightImpact;
    
    [Tooltip("Preset for wrong letter touch (medium intensity)")]
    [SerializeField] private HapticPatterns.PresetType wrongLetterPreset = HapticPatterns.PresetType.Warning;
    
    [Tooltip("Preset for word completion (strong)")]
    [SerializeField] private HapticPatterns.PresetType wordCompletePreset = HapticPatterns.PresetType.Success;
    
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Initialize the haptic system
        HapticController.Init();
    }
    
    /// <summary>
    /// Checks if vibration is enabled in settings
    /// </summary>
    private bool IsVibrationEnabled()
    {
        return enableVibration && SettingsManager.GetVibrationEnabled();
    }
    
    /// <summary>
    /// Triggers a light vibration when player collects a correct letter
    /// </summary>
    public void VibrateCorrectLetter()
    {
        if (!IsVibrationEnabled()) return;
        
        MMVibrationManager.Haptic (HapticTypes.LightImpact);
        HapticPatterns.PlayPreset(correctLetterPreset);
    }
    
    /// <summary>
    /// Triggers a medium vibration when player touches a wrong letter
    /// </summary>
    public void VibrateWrongLetter()
    {
        if (!IsVibrationEnabled()) return;
        
        MMVibrationManager.Haptic (HapticTypes.Warning);
        HapticPatterns.PlayPreset(wrongLetterPreset);
    }
    
    /// <summary>
    /// Triggers a strong vibration when player completes a word
    /// </summary>
    public void VibrateWordComplete()
    {
        if (!IsVibrationEnabled()) return;
        
        MMVibrationManager.Haptic (HapticTypes.Success);
        HapticPatterns.PlayPreset(wordCompletePreset);
    }
    
    /// <summary>
    /// Plays a custom haptic preset
    /// </summary>
    /// <param name="preset">The preset type to play</param>
    public void PlayCustomPreset(HapticPatterns.PresetType preset)
    {
        if (!IsVibrationEnabled()) return;
        
        HapticPatterns.PlayPreset(preset);
    }
    
    /// <summary>
    /// Plays a custom emphasis haptic with specific amplitude and frequency
    /// </summary>
    /// <param name="amplitude">Amplitude from 0.0 to 1.0</param>
    /// <param name="frequency">Frequency from 0.0 to 1.0</param>
    public void PlayEmphasis(float amplitude, float frequency)
    {
        if (!IsVibrationEnabled()) return;
        
        HapticPatterns.PlayEmphasis(amplitude, frequency);
    }
    
    /// <summary>
    /// Plays a constant haptic with specific parameters
    /// </summary>
    /// <param name="amplitude">Amplitude from 0.0 to 1.0</param>
    /// <param name="frequency">Frequency from 0.0 to 1.0</param>
    /// <param name="duration">Duration in seconds</param>
    public void PlayConstant(float amplitude, float frequency, float duration)
    {
        if (!IsVibrationEnabled()) return;
        
        HapticPatterns.PlayConstant(amplitude, frequency, duration);
    }
    
    /// <summary>
    /// Stops any currently playing haptic
    /// </summary>
    public void StopVibration()
    {
        HapticController.Stop();
    }
    
    void OnApplicationFocus(bool hasFocus)
    {
        // Stop vibration when app loses focus
        HapticController.ProcessApplicationFocus(hasFocus);
    }
}
