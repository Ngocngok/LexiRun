using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameConfig config;
    public GameObject letterNodePrefab;
    public GameObject playerPrefab;
    public GameObject botPrefab;
    
    public Transform arenaParent;
    public Transform actorsParent;
    
    private PlayerController player;
    private List<BotController> bots = new List<BotController>();
    private List<LetterNode> letterNodes = new List<LetterNode>();
    
    private bool gameActive = false;
    private List<string> availableWords = new List<string>();
    private List<string> usedWords = new List<string>();
    
    private UIManager uiManager;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        InitializeGame();
    }
    
    void InitializeGame()
    {
        availableWords = new List<string>(config.wordList);
        
        CreateArena();
        CreatePlayer();
        CreateBots();
        
        StartGame();
    }
    
    void CreateArena()
    {
        if (arenaParent == null)
        {
            GameObject arenaObj = new GameObject("Arena");
            arenaParent = arenaObj.transform;
        }
        
        // Create 26 letter nodes (A-Z) randomly positioned in the arena
        char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        float halfSize = config.arenaSize / 2f;
        
        List<Vector3> positions = new List<Vector3>();
        
        for (int i = 0; i < 26; i++)
        {
            Vector3 position;
            int attempts = 0;
            
            do
            {
                position = new Vector3(
                    Random.Range(-halfSize, halfSize),
                    0.5f,
                    Random.Range(-halfSize, halfSize)
                );
                attempts++;
            }
            while (IsTooClose(position, positions, config.nodeSpacing) && attempts < 100);
            
            positions.Add(position);
            
            GameObject nodeObj = Instantiate(letterNodePrefab, position, Quaternion.identity, arenaParent);
            nodeObj.name = "Node_" + alphabet[i];
            
            LetterNode node = nodeObj.GetComponent<LetterNode>();
            node.Initialize(alphabet[i]);
            letterNodes.Add(node);
        }
    }
    
    bool IsTooClose(Vector3 position, List<Vector3> existingPositions, float minDistance)
    {
        foreach (Vector3 existing in existingPositions)
        {
            if (Vector3.Distance(position, existing) < minDistance)
            {
                return true;
            }
        }
        return false;
    }
    
    void CreatePlayer()
    {
        if (actorsParent == null)
        {
            GameObject actorsObj = new GameObject("Actors");
            actorsParent = actorsObj.transform;
        }
        
        Vector3 spawnPos = new Vector3(0, 1, -config.arenaSize / 3f);
        GameObject playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity, actorsParent);
        playerObj.name = "Player";
        
        player = playerObj.GetComponent<PlayerController>();
        player.Initialize(0, "Player", Color.green, config.playerMoveSpeed);
    }
    
    void CreateBots()
    {
        Color[] botColors = { Color.red, Color.blue, Color.yellow, Color.magenta };
        
        for (int i = 0; i < config.botCount; i++)
        {
            float angle = (i + 1) * (360f / (config.botCount + 1)) * Mathf.Deg2Rad;
            float radius = config.arenaSize / 3f;
            Vector3 spawnPos = new Vector3(
                Mathf.Cos(angle) * radius,
                1,
                Mathf.Sin(angle) * radius
            );
            
            GameObject botObj = Instantiate(botPrefab, spawnPos, Quaternion.identity, actorsParent);
            botObj.name = "Bot_" + (i + 1);
            
            BotController bot = botObj.GetComponent<BotController>();
            bot.Initialize(i + 1, "Bot " + (i + 1), botColors[i % botColors.Length], config.botMoveSpeed);
            bots.Add(bot);
        }
    }
    
    void StartGame()
    {
        gameActive = true;
        
        // Assign starting words
        AssignNewWord(player);
        foreach (BotController bot in bots)
        {
            AssignNewWord(bot);
        }
        
        if (uiManager != null)
        {
            uiManager.Initialize(player, bots);
        }
    }
    
    public void AssignNewWord(ActorController actor)
    {
        if (availableWords.Count == 0)
        {
            // Refill word list
            availableWords = new List<string>(config.wordList);
            availableWords = availableWords.Except(usedWords).ToList();
            
            if (availableWords.Count == 0)
            {
                availableWords = new List<string>(config.wordList);
                usedWords.Clear();
            }
        }
        
        int randomIndex = Random.Range(0, availableWords.Count);
        string word = availableWords[randomIndex];
        availableWords.RemoveAt(randomIndex);
        usedWords.Add(word);
        
        actor.AssignWord(word);
    }
    
    public void OnActorWon(ActorController actor)
    {
        if (!gameActive) return;
        
        gameActive = false;
        
        if (actor == player)
        {
            if (uiManager != null)
            {
                uiManager.ShowGameOver(true, "You Won!");
            }
        }
        else
        {
            if (uiManager != null)
            {
                uiManager.ShowGameOver(false, actor.actorName + " won!");
            }
        }
    }
    
    public void OnPlayerLost(string reason)
    {
        if (!gameActive) return;
        
        gameActive = false;
        player.isEliminated = true;
        
        if (uiManager != null)
        {
            uiManager.ShowGameOver(false, "You Lost! " + reason);
        }
    }
    
    public bool IsGameActive()
    {
        return gameActive;
    }
    
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
