using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    
    [Header("SFX Clips")]
    public AudioClip buttonClickSFX;
    public AudioClip correctLetterSFX;
    public AudioClip wrongLetterSFX;
    public AudioClip wordCompleteSFX;
    public AudioClip gameWinSFX;
    public AudioClip gameLoseSFX;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Create audio sources if not assigned
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
            }
            
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.loop = false;
            }
            
            // Load settings
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void LoadSettings()
    {
        bool musicEnabled = SettingsManager.GetMusicEnabled();
        bool sfxEnabled = SettingsManager.GetSFXEnabled();
        
        musicSource.mute = !musicEnabled;
        sfxSource.mute = !sfxEnabled;
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            if (musicSource.clip != clip)
            {
                musicSource.clip = clip;
                musicSource.Play();
            }
        }
    }
    
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    public void SetMusicEnabled(bool enabled)
    {
        if (musicSource != null)
        {
            musicSource.mute = !enabled;
        }
    }
    
    public void SetSFXEnabled(bool enabled)
    {
        if (sfxSource != null)
        {
            sfxSource.mute = !enabled;
        }
    }
    
    // Convenience methods for specific sounds
    public void PlayButtonClick() => PlaySFX(buttonClickSFX);
    public void PlayCorrectLetter() => PlaySFX(correctLetterSFX);
    public void PlayWrongLetter() => PlaySFX(wrongLetterSFX);
    public void PlayWordComplete() => PlaySFX(wordCompleteSFX);
    public void PlayGameWin() => PlaySFX(gameWinSFX);
    public void PlayGameLose() => PlaySFX(gameLoseSFX);
}
