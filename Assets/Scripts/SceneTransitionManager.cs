using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    
    private int levelToLoad = 1;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadLoadingScene()
    {
        SceneManager.LoadScene("LoadingScene");
    }
    
    public void LoadHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }
    
    public void LoadGameplayScene(int level)
    {
        levelToLoad = level;
        SceneManager.LoadScene("GameplayScene");
    }
    
    public int GetLevelToLoad()
    {
        return levelToLoad;
    }
}
