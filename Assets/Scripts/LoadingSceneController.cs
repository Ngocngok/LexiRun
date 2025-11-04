using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    public Slider progressBar;
    public Text progressText;
    public float loadingDuration = 2f;
    
    void Start()
    {
        // Ensure Time.timeScale is reset (in case it was paused in previous scene)
        Time.timeScale = 1f;
        
        StartCoroutine(LoadingSequence());
    }
    
    IEnumerator LoadingSequence()
    {
        float elapsed = 0f;
        
        while (elapsed < loadingDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaledDeltaTime to work even if timeScale = 0
            float progress = Mathf.Clamp01(elapsed / loadingDuration);
            
            if (progressBar != null)
            {
                progressBar.value = progress;
            }
            
            if (progressText != null)
            {
                progressText.text = Mathf.RoundToInt(progress * 100f) + "%";
            }
            
            yield return null;
        }
        
        // Ensure we reach 100%
        if (progressBar != null) progressBar.value = 1f;
        if (progressText != null) progressText.text = "100%";
        
        yield return new WaitForSeconds(0.5f);
        
        // Load home scene
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadHomeScene();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("HomeScene");
        }
    }
}
