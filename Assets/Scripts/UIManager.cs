using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Player HUD")]
    public Text playerWordText;
    public Text playerHPText;
    public Text playerTimerText;
    
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
    
    private PlayerController player;
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
            playerWordText.text = "Your Word: " + player.wordProgress.currentWord;
        }
        
        if (playerHPText != null)
        {
            playerHPText.text = "HP: " + Mathf.Max(0, (int)player.currentHP);
        }
        
        if (playerTimerText != null)
        {
            int seconds = Mathf.Max(0, Mathf.CeilToInt(player.currentTime));
            playerTimerText.text = "Time: " + seconds + "s";
            
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
