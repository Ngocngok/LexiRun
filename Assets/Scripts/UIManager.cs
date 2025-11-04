using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Player HUD")]
    public Text playerWordText;
    public Text playerHPText;
    public Text playerTimerText;
    public GameObject[] hearts;
    
    [Header("Bot Info")]
    public Transform botInfoParent;
    public GameObject botInfoPrefab;
    
    [Header("Victory Screen")]
    public GameObject victoryPanel;
    public Text victoryText;
    public Button nextLevelButton;
    public Button victoryHomeButton;
    
    [Header("Lose Screen")]
    public GameObject losePanel;
    public Text loseText;
    public Button retryButton;
    public Button loseHomeButton;
    
    [Header("Pause Screen")]
    public Button pauseButton;
    public GameObject pausePanel;
    public Button pauseResumeButton;
    public Button pauseHomeButton;
    
    [Header("Tutorial")]
    public GameObject tutorialPanel;
    public GameObject tutorialSlide1;
    public GameObject tutorialSlide2;
    public GameObject tutorialSlide3;
    public Image tutorialImage1;
    public Image tutorialImage2;
    public Image tutorialImage3;
    public Button tutorialNextButton1;
    public Button tutorialNextButton2;
    public Button tutorialOKButton;
    
    private PlayerController player;
    private int currentTutorialSlide = 1;
    private List<BotController> bots;
    private Dictionary<BotController, BotInfoUI> botInfoUIs = new Dictionary<BotController, BotInfoUI>();
    
    void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
        
        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }
        
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(OnNextLevelClicked);
        }
        
        if (victoryHomeButton != null)
        {
            victoryHomeButton.onClick.AddListener(OnHomeClicked);
        }
        
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryClicked);
        }
        
        if (loseHomeButton != null)
        {
            loseHomeButton.onClick.AddListener(OnHomeClicked);
        }
        
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OnPauseClicked);
        }
        
        if (pauseResumeButton != null)
        {
            pauseResumeButton.onClick.AddListener(OnResumeClicked);
        }
        
        if (pauseHomeButton != null)
        {
            pauseHomeButton.onClick.AddListener(OnPauseHomeClicked);
        }
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        
        // Setup tutorial buttons
        if (tutorialNextButton1 != null)
        {
            tutorialNextButton1.onClick.AddListener(OnTutorialNext1);
        }
        
        if (tutorialNextButton2 != null)
        {
            tutorialNextButton2.onClick.AddListener(OnTutorialNext2);
        }
        
        if (tutorialOKButton != null)
        {
            tutorialOKButton.onClick.AddListener(OnTutorialOK);
        }
        
        
    }
    
    public void Initialize(PlayerController player, List<BotController> bots)
    {
        this.player = player;
        this.bots = bots;
        
        // Create bot info UIs
        if (botInfoParent != null && botInfoPrefab != null)
        {
            foreach (BotController bot in bots)
            {
                GameObject botInfoObj = Instantiate(botInfoPrefab, botInfoParent);
                BotInfoUI botInfo = botInfoObj.GetComponent<BotInfoUI>();
                if (botInfo != null)
                {
                    botInfo.Initialize(bot);
                    botInfoUIs[bot] = botInfo;
                }
            }
        }
    }
    
    void Update()
    {
        if (player != null)
        {
            UpdatePlayerHUD();
        }
        
        UpdateBotInfo();
    }
    
    void UpdatePlayerHUD()
    {
        if (playerWordText != null)
        {
            playerWordText.text = player.wordProgress.currentWord;
        }
        
        for (int i = 0; i < 3; i++)
        {
            hearts[i].SetActive(i < player.currentHP);
        }
        //playerHPText.text = "HP: " + Mathf.Max(0, (int)player.currentHP);
        
        
        if (playerTimerText != null)
        {
            int seconds = Mathf.Max(0, Mathf.CeilToInt(player.currentTime));
            playerTimerText.text = seconds + "s";
            
            // Warning color when time is low
            if (seconds <= 10)
            {
                playerTimerText.color = Color.red;
            }
            else
            {
                playerTimerText.color = Color.white;
            }
        }
    }
    
    void UpdateBotInfo()
    {
        foreach (var kvp in botInfoUIs)
        {
            if (kvp.Value != null)
            {
                kvp.Value.UpdateInfo();
            }
        }
    }
    
    public void ShowVictoryScreen(int completedLevel)
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        
        if (victoryText != null)
        {
            victoryText.text = "Level " + completedLevel + " Complete!\nYou Won!";
        }
    }
    
    public void ShowLoseScreen(string reason)
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
        
        if (loseText != null)
        {
            loseText.text = "You Lost!\n" + reason;
        }
    }
    
    void OnNextLevelClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        int nextLevel = SettingsManager.GetCurrentLevel();
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadGameplayScene(nextLevel);
        }
    }
    
    void OnRetryClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    
    void OnHomeClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadHomeScene();
        }
    }
    
    void OnPauseClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        PauseGame();
    }
    
    void OnResumeClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        ResumeGame();
    }
    
    void OnPauseHomeClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Resume game before going home (to reset Time.timeScale)
        ResumeGame();
        
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadHomeScene();
        }
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause game
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        
        // Note: Music continues playing (not affected by Time.timeScale)
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume game
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }
    
    // Tutorial Methods
    public void ShowTutorial()
    {
        if (tutorialPanel != null)
        {
            currentTutorialSlide = 1;
            tutorialPanel.SetActive(true);
            ShowTutorialSlide(1);
            
            // Pause game during tutorial
            Time.timeScale = 0f;
        }
    }
    
    private void ShowTutorialSlide(int slideNumber)
    {
        // Hide all slides
        if (tutorialSlide1 != null) tutorialSlide1.SetActive(false);
        if (tutorialSlide2 != null) tutorialSlide2.SetActive(false);
        if (tutorialSlide3 != null) tutorialSlide3.SetActive(false);
        
        // Show requested slide
        switch (slideNumber)
        {
            case 1:
                if (tutorialSlide1 != null) tutorialSlide1.SetActive(true);
                break;
            case 2:
                if (tutorialSlide2 != null) tutorialSlide2.SetActive(true);
                break;
            case 3:
                if (tutorialSlide3 != null) tutorialSlide3.SetActive(true);
                break;
        }
        
        currentTutorialSlide = slideNumber;
    }
    
    void OnTutorialNext1()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        ShowTutorialSlide(2);
    }
    
    void OnTutorialNext2()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        ShowTutorialSlide(3);
    }
    
    void OnTutorialOK()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        // Hide tutorial
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        
        // Mark tutorial as completed
        SettingsManager.SetTutorialCompleted(true);
        
        // Resume game
        Time.timeScale = 1f;
    }
}

public class BotInfoUI : MonoBehaviour
{
    public Text botNameText;
    public Text botWordText;
    public Text botMistakesText;
    public Text botWordsCompletedText;
    
    private BotController bot;
    
    public void Initialize(BotController bot)
    {
        this.bot = bot;
        
        if (botNameText != null)
        {
            botNameText.text = bot.actorName;
            botNameText.color = bot.actorColor;
        }
    }
    
    public void UpdateInfo()
    {
        if (bot == null) return;
        
        if (bot.isEliminated)
        {
            if (botWordText != null) botWordText.text = "ELIMINATED";
            if (botMistakesText != null) botMistakesText.text = "";
            if (botWordsCompletedText != null) botWordsCompletedText.text = "";
            return;
        }
        
        // Hide word text since it's now floating above the bot
        if (botWordText != null)
        {
            botWordText.text = "";
        }
        
        if (botMistakesText != null)
        {
            botMistakesText.text = "Mistakes: " + bot.mistakeCount + "/3";
        }
        
        if (botWordsCompletedText != null)
        {
            botWordsCompletedText.text = "Words: " + bot.completedWords + "/3";
        }
    }
}
