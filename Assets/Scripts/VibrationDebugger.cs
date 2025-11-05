using UnityEngine;
using Lofelt.NiceVibrations;

/// <summary>
/// Debug tool to diagnose vibration issues.
/// Attach to a GameObject and check the console for diagnostic information.
/// </summary>
public class VibrationDebugger : MonoBehaviour
{
    [Header("Test Controls")]
    [SerializeField] private bool testOnStart = true;
    [SerializeField] private KeyCode testKey = KeyCode.Space;
    
    void Start()
    {
        if (testOnStart)
        {
            RunDiagnostics();
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(testKey))
        {
            TestVibration();
        }
    }
    
    public void RunDiagnostics()
    {
        Debug.Log("=== VIBRATION DIAGNOSTICS ===");
        
        // Check VibrationManager
        Debug.Log($"VibrationManager.Instance exists: {VibrationManager.Instance != null}");
        
        // Check Settings
        Debug.Log($"Settings - Vibration Enabled: {SettingsManager.GetVibrationEnabled()}");
        
        // Check HapticController
        bool initialized = HapticController.Init();
        Debug.Log($"HapticController initialized: {initialized}");
        Debug.Log($"HapticController.hapticsEnabled: {HapticController.hapticsEnabled}");
        
        // Check Device Capabilities
        Debug.Log($"DeviceCapabilities.isVersionSupported: {DeviceCapabilities.isVersionSupported}");
        Debug.Log($"DeviceCapabilities.meetsAdvancedRequirements: {DeviceCapabilities.meetsAdvancedRequirements}");
        
        // Platform info
        Debug.Log($"Platform: {Application.platform}");
        Debug.Log($"Is Mobile: {Application.isMobilePlatform}");
        
#if UNITY_ANDROID
        Debug.Log("Build Platform: Android");
#elif UNITY_IOS
        Debug.Log("Build Platform: iOS");
#else
        Debug.Log("Build Platform: Other (vibration may not work)");
#endif
        
        Debug.Log("=== END DIAGNOSTICS ===");
    }
    
    public void TestVibration()
    {
        Debug.Log("Testing vibration...");
        
        if (VibrationManager.Instance != null)
        {
            Debug.Log("Playing Warning preset via VibrationManager");
            VibrationManager.Instance.VibrateWrongLetter();
        }
        else
        {
            Debug.Log("VibrationManager.Instance is null! Playing directly...");
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
        }
        
        Debug.Log("Vibration test complete. Did you feel it?");
    }
}
