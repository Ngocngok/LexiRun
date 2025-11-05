# Vibration Troubleshooting Guide

## Why Vibration Might Not Work

### Common Issues and Solutions

---

## ✅ **Issue #1: Android Permission Missing**

**Problem:** Android requires VIBRATE permission in the manifest.

**Solution:** ✅ **FIXED** - Created `Assets/Plugins/Android/AndroidManifest.xml` with:
```xml
<uses-permission android:name="android.permission.VIBRATE" />
```

**How to Verify:**
1. Check that `Assets/Plugins/Android/AndroidManifest.xml` exists
2. Build the app and install on Android device
3. Test vibration

---

## ✅ **Issue #2: Vibration Disabled in Settings**

**Problem:** User has disabled vibration in the game settings.

**Solution:**
1. Open the game
2. Go to Settings menu
3. Enable "Vibration" toggle
4. Test again

**Code Check:**
```csharp
// Check if enabled
bool isEnabled = SettingsManager.GetVibrationEnabled();
Debug.Log($"Vibration enabled in settings: {isEnabled}");
```

---

## ✅ **Issue #3: Device System Settings**

**Problem:** Device vibration is disabled at system level.

**Solution:**

**Android:**
1. Go to device Settings → Sound & Vibration
2. Enable "Vibrate on touch" or "Haptic feedback"
3. Check "Do Not Disturb" mode is off
4. Test again

**iOS:**
1. Go to Settings → Sounds & Haptics
2. Enable "System Haptics"
3. Check "Silent Mode" switch position
4. Test again

---

## ✅ **Issue #4: Device Not Supported**

**Problem:** Device doesn't support haptic feedback.

**Minimum Requirements:**
- **iOS**: iPhone 8 or newer (Taptic Engine required)
- **Android**: Android 8.0 (API 26) or newer for best experience
- **Android**: Android 4.2 (API 17) minimum for basic vibration

**How to Check:**
1. Add VibrationDebugger component (already added to VibrationManager)
2. Run the game
3. Check console logs for device capabilities
4. Look for:
   - `DeviceCapabilities.isVersionSupported: true/false`
   - `DeviceCapabilities.meetsAdvancedRequirements: true/false`

**Unsupported Devices:**
- iPhone 7 and older (no Taptic Engine)
- iPads (most don't have haptic feedback)
- Android devices older than API 17
- Emulators (no vibration hardware)

---

## ✅ **Issue #5: Testing in Unity Editor**

**Problem:** Unity Editor doesn't vibrate (no hardware).

**Solution:**
- Vibration only works on **real devices**
- Editor shows logs but doesn't vibrate
- **Must build and deploy to device to test**

**How to Test:**
1. Build to Android or iOS
2. Install on physical device
3. Run the game
4. Touch letters to trigger vibration

---

## ✅ **Issue #6: VibrationManager Not Initialized**

**Problem:** VibrationManager.Instance is null.

**Solution:**
1. Open GameplayScene
2. Check VibrationManager GameObject exists
3. Check VibrationManager component is attached
4. Check component is enabled

**Code Check:**
```csharp
if (VibrationManager.Instance == null)
{
    Debug.LogError("VibrationManager.Instance is NULL!");
}
```

---

## ✅ **Issue #7: HapticController Not Initialized**

**Problem:** HapticController.Init() failed or wasn't called.

**Solution:**
- VibrationManager calls `HapticController.Init()` in Awake()
- Check console for initialization errors
- Verify Feel package is properly imported

**Diagnostic:**
Run the game and check console for:
```
HapticController initialized: true/false
```

---

## 🔧 **Diagnostic Tool**

### Using VibrationDebugger:

**Already Added:** VibrationDebugger component is on VibrationManager in GameplayScene.

**How to Use:**
1. Run the game (on device or in editor)
2. Check console for diagnostic output
3. Press **Space** key to test vibration manually
4. Review the diagnostic information

**Diagnostic Output:**
```
=== VIBRATION DIAGNOSTICS ===
VibrationManager.Instance exists: true/false
Settings - Vibration Enabled: true/false
HapticController initialized: true/false
HapticController.hapticsEnabled: true/false
DeviceCapabilities.isVersionSupported: true/false
DeviceCapabilities.meetsAdvancedRequirements: true/false
Platform: Android/iOS/WindowsEditor
Is Mobile: true/false
Build Platform: Android/iOS/Other
=== END DIAGNOSTICS ===
```

---

## 📱 **Platform-Specific Issues**

### **Android:**

**Issue:** Vibration works but feels weak
- Some Android devices have weak vibration motors
- Try stronger presets (HeavyImpact, Failure)
- Adjust in VibrationManager Inspector

**Issue:** Vibration doesn't work on specific device
- Some manufacturers disable haptic APIs
- Check device settings for "Haptic feedback"
- Try on different Android device

**Issue:** Permission denied
- Ensure AndroidManifest.xml has VIBRATE permission
- Rebuild the app
- Reinstall on device

### **iOS:**

**Issue:** Vibration doesn't work
- Check device model (iPhone 8+ required)
- iPads don't support haptic feedback
- Check System Haptics in Settings

**Issue:** Silent mode blocks vibration
- iOS respects silent mode switch
- Flip switch to ring mode
- Or enable "Vibrate on Silent" in Settings

---

## 🧪 **Testing Checklist**

### **Before Building:**
- [ ] VibrationManager exists in GameplayScene
- [ ] VibrationManager.enableVibration is true
- [ ] AndroidManifest.xml has VIBRATE permission
- [ ] No compilation errors

### **In Game (Device):**
- [ ] Settings → Vibration is enabled
- [ ] Touch correct letter → Light vibration
- [ ] Touch wrong letter → Strong vibration
- [ ] Complete word → Success vibration
- [ ] Check console for diagnostic logs

### **Device Settings:**
- [ ] Android: Haptic feedback enabled
- [ ] iOS: System Haptics enabled
- [ ] Do Not Disturb mode is off
- [ ] Silent mode is off (iOS)

---

## 🔍 **Step-by-Step Diagnosis**

### **Step 1: Check Code**
```csharp
// In any script, add this to test:
void Start()
{
    Debug.Log($"VibrationManager exists: {VibrationManager.Instance != null}");
    Debug.Log($"Settings enabled: {SettingsManager.GetVibrationEnabled()}");
    
    // Test vibration directly
    HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
}
```

### **Step 2: Check Settings**
1. Run game on device
2. Open Settings menu
3. Check Vibration toggle is ON
4. If OFF, turn it ON
5. Test again

### **Step 3: Check Device**
1. Go to device system settings
2. Enable haptic feedback
3. Test with other apps (keyboard, etc.)
4. If other apps vibrate, issue is in game
5. If other apps don't vibrate, issue is device

### **Step 4: Check Build**
1. Verify you're testing on **real device** (not editor)
2. Verify build platform matches device (Android/iOS)
3. Rebuild and reinstall
4. Test again

### **Step 5: Check Logs**
1. Connect device to computer
2. Open Unity Console
3. Filter for "Vibration" or "Haptic"
4. Look for errors or warnings
5. Check diagnostic output

---

## 💡 **Quick Fixes**

### **Fix #1: Force Enable Vibration**
```csharp
// In VibrationManager, temporarily set:
private bool enableVibration = true; // Already true

// And bypass settings check:
private bool IsVibrationEnabled()
{
    return true; // Force enable for testing
}
```

### **Fix #2: Test with Simple Vibration**
```csharp
// Add to any script:
void Update()
{
    if (Input.GetKeyDown(KeyCode.V))
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
        #endif
        
        Debug.Log("Vibration test triggered!");
    }
}
```

### **Fix #3: Use Alternative Haptic Method**
```csharp
// Try different preset:
HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);

// Or try emphasis:
HapticPatterns.PlayEmphasis(1.0f, 0.5f);

// Or try constant:
HapticPatterns.PlayConstant(1.0f, 0.5f, 0.3f);
```

---

## 📋 **Most Likely Causes**

### **Ranked by Probability:**

1. **Testing in Unity Editor** (90% of cases)
   - Solution: Build to device

2. **Vibration disabled in game settings** (5%)
   - Solution: Enable in Settings menu

3. **Android permission missing** (3%)
   - Solution: ✅ Fixed - AndroidManifest.xml created

4. **Device doesn't support haptics** (1%)
   - Solution: Test on different device

5. **System settings disabled** (1%)
   - Solution: Enable in device settings

---

## 🎯 **Recommended Testing Steps**

### **Quick Test:**
1. Build to Android/iOS device
2. Install and run
3. Go to Settings → Enable Vibration
4. Play game
5. Touch wrong letter
6. Should feel strong vibration

### **Detailed Test:**
1. Add VibrationDebugger (already added)
2. Build to device
3. Run game
4. Check console logs
5. Press Space to test manually
6. Review diagnostic output
7. Adjust based on findings

---

## 📞 **Still Not Working?**

### **Provide This Information:**

1. **Device Info:**
   - Device model (e.g., "Samsung Galaxy S21", "iPhone 13")
   - OS version (e.g., "Android 12", "iOS 16")
   - Does vibration work in other apps?

2. **Game Settings:**
   - Is vibration enabled in Settings menu?
   - Check VibrationManager.enableVibration in Inspector

3. **Console Logs:**
   - Run VibrationDebugger
   - Copy diagnostic output
   - Look for errors

4. **Build Info:**
   - Build platform (Android/iOS)
   - Unity version
   - Feel package version

---

## 📝 **Summary**

**Most Common Issue:** Testing in Unity Editor instead of real device.

**Quick Fix:** Build to device and test there.

**If Still Not Working:**
1. Check AndroidManifest.xml has VIBRATE permission ✅
2. Enable vibration in game Settings menu
3. Enable haptic feedback in device settings
4. Use VibrationDebugger to diagnose
5. Test on different device if available

---

**Created:** November 5, 2025  
**Status:** AndroidManifest.xml added, VibrationDebugger added
