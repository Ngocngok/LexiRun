using UnityEngine;

public static class SettingsManager
{
    private const string MUSIC_KEY = "MusicEnabled";
    private const string SFX_KEY = "SFXEnabled";
    private const string VIBRATION_KEY = "VibrationEnabled";
    private const string CURRENT_LEVEL_KEY = "CurrentLevel";
    private const string TUTORIAL_COMPLETED_KEY = "TutorialCompleted";
    
    // Music Settings
    public static bool GetMusicEnabled()
    {
        return PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
    }
    
    public static void SetMusicEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(MUSIC_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    // SFX Settings
    public static bool GetSFXEnabled()
    {
        return PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
    }
    
    public static void SetSFXEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(SFX_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    // Vibration Settings
    public static bool GetVibrationEnabled()
    {
        return PlayerPrefs.GetInt(VIBRATION_KEY, 1) == 1;
    }
    
    public static void SetVibrationEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(VIBRATION_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    // Level Progress
    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 1);
    }
    
    public static void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, level);
        PlayerPrefs.Save();
    }
    
    public static void UnlockNextLevel()
    {
        int currentLevel = GetCurrentLevel();
        SetCurrentLevel(currentLevel + 1);
    }
    
    public static void ResetProgress()
    {
        SetCurrentLevel(1);
    }
    
    // Tutorial Settings
    public static bool GetTutorialCompleted()
    {
        return PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY, 0) == 1;
    }
    
    public static void SetTutorialCompleted(bool completed)
    {
        PlayerPrefs.SetInt(TUTORIAL_COMPLETED_KEY, completed ? 1 : 0);
        PlayerPrefs.Save();
    }
}
