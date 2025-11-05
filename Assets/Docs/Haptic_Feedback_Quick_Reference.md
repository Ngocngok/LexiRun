# Haptic Feedback - Quick Reference Guide

## Quick Overview

LexiRun now includes haptic feedback (vibration) using the Feel package (Nice Vibrations). The system provides three levels of tactile feedback:

| Event | Vibration Type | Preset | Intensity |
|-------|---------------|--------|-----------|
| **Correct Letter** | Light tap | LightImpact | Subtle ⚪ |
| **Wrong Letter** | Warning pulse | Warning | Medium 🟡 |
| **Word Complete** | Success pattern | Success | Strong 🟢 |

---

## For Designers

### Adjusting Vibration Intensity

1. Open `GameplayScene`
2. Select `VibrationManager` GameObject
3. In Inspector, change the preset dropdowns:
   - **Correct Letter Preset**
   - **Wrong Letter Preset**
   - **Word Complete Preset**

### Available Presets (Ordered by Intensity)

**Light:**
- Selection (lightest)
- Light Impact
- Soft Impact

**Medium:**
- Medium Impact
- Rigid Impact

**Strong:**
- Heavy Impact
- Warning
- Success
- Failure (strongest)

### Testing

- Vibration only works on real devices (iOS/Android) or gamepads
- Unity Editor doesn't vibrate (simulated only)
- Build to device to test properly
- Check Settings menu to enable/disable vibration

---

## For Programmers

### Basic Usage

```csharp
// Trigger predefined vibrations
VibrationManager.Instance.VibrateCorrectLetter();
VibrationManager.Instance.VibrateWrongLetter();
VibrationManager.Instance.VibrateWordComplete();

// Play custom preset
VibrationManager.Instance.PlayCustomPreset(HapticPatterns.PresetType.HeavyImpact);

// Custom emphasis (amplitude, frequency from 0.0 to 1.0)
VibrationManager.Instance.PlayEmphasis(0.8f, 0.5f);

// Constant vibration (amplitude, frequency, duration in seconds)
VibrationManager.Instance.PlayConstant(0.5f, 0.3f, 0.2f);

// Stop vibration
VibrationManager.Instance.StopVibration();
```

### Adding New Vibration Events

1. Open the script where you want to add vibration
2. Add the vibration call:
```csharp
if (VibrationManager.Instance != null)
{
    VibrationManager.Instance.PlayCustomPreset(HapticPatterns.PresetType.MediumImpact);
}
```

### Settings Integration

Vibration automatically respects the user's settings:
```csharp
// Check if vibration is enabled
bool isEnabled = SettingsManager.GetVibrationEnabled();

// Enable/disable vibration
SettingsManager.SetVibrationEnabled(true);
```

---

## For QA/Testers

### Test Checklist

- [ ] Correct letter touch vibrates lightly
- [ ] Wrong letter touch vibrates harder
- [ ] Word completion vibrates strongly
- [ ] Vibration can be disabled in Settings
- [ ] Vibration respects settings (on/off)
- [ ] Multiple vibrations don't overlap badly
- [ ] Vibration stops when app loses focus
- [ ] Works on iOS devices (iPhone 8+)
- [ ] Works on Android devices (8.0+)
- [ ] Works with gamepads (if applicable)

### Common Issues

**No vibration on device:**
- Check Settings → Vibration is enabled
- Verify device supports haptics (iPhone 8+, Android 8.0+)
- Check device's system vibration settings
- Restart the app

**Vibration too strong/weak:**
- Report to designers for preset adjustment
- Note device model (varies by manufacturer)

**Vibration continues after event:**
- Report as bug (should stop automatically)
- Check if app loses focus properly

---

## Platform Support

| Platform | Support | Notes |
|----------|---------|-------|
| iOS | ✅ Full | iPhone 8+ with Taptic Engine |
| Android | ✅ Full | Android 8.0+ for best experience |
| Gamepad | ✅ Full | Xbox, PlayStation, Switch Pro |
| Unity Editor | ⚠️ Simulated | No actual vibration |
| WebGL | ❌ No | Not supported |

---

## Files Modified

- `Assets/Scripts/VibrationManager.cs` - New manager script
- `Assets/Scripts/PlayerController.cs` - Added vibration triggers
- `Assets/Scenes/GameplayScene.unity` - Added VibrationManager GameObject

---

## Documentation

For detailed information, see:
- `Assets/Docs/Vibration_Integration.md` - Complete implementation details
- `Assets/Feel/` - Feel package documentation

---

**Last Updated:** November 5, 2025
