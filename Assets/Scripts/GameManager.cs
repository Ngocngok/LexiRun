using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameConfig config;
    public LevelConfig levelConfig;
    public GameObject letterNodePrefab;
    public GameObject playerPrefab;
    public GameObject[] botPrefabs; // Array of different bot prefabs
    public Vector3[] botPositions;
    
    public Transform arenaParent;
    public Transform actorsParent;
    
    private PlayerController player;
    private List<BotController> bots = new List<BotController>();
    private List<LetterNode> letterNodes = new List<LetterNode>();
    
    private bool gameActive = false;
    private List<string> availableWords = new List<string>();
    private List<string> usedWords = new List<string>();
    
    private UIManager uiManager;
    private int currentLevel = 1;
    private DifficultySettings currentDifficulty;
    
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
        
        // Get current level from scene transition or settings
        if (SceneTransitionManager.Instance != null)
        {
            currentLevel = SceneTransitionManager.Instance.GetLevelToLoad();
        }
        else
        {
            currentLevel = SettingsManager.GetCurrentLevel();
        }
        
        // Get difficulty settings for this level
        if (levelConfig != null)
        {
            currentDifficulty = levelConfig.GetSettingsForLevel(currentLevel);
        }
        
        InitializeGame();
    }
    
    void InitializeGame()
    {
        // Filter words based on difficulty
        FilterWordsByDifficulty();
        
        CreateArena();
        CreatePlayer();
        CreateBots();
        
        StartGame();
        
        // Play gameplay music
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameplayMusic);
        }
    }
    
    void FilterWordsByDifficulty()
    {
        availableWords.Clear();
        
        if (currentDifficulty != null)
        {
            foreach (string word in config.wordList)
            {
                if (word.Length >= currentDifficulty.minWordLength && 
                    word.Length <= currentDifficulty.maxWordLength)
                {
                    availableWords.Add(word);
                }
            }
        }
        
        if (availableWords.Count == 0)
        {
            // Fallback to all words if no words match
            availableWords = new List<string>(config.wordList);
        }
    }
    
    void CreateArena()
    {
        if (arenaParent == null)
        {
            GameObject arenaObj = new GameObject("Arena");
            arenaParent = arenaObj.transform;
        }
        
        // Create 26 letter nodes (A-Z) in a 4x7 grid with small random offsets
        char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        
        // Shuffle the alphabet so letters aren't in A-Z order
        for (int i = alphabet.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            char temp = alphabet[i];
            alphabet[i] = alphabet[randomIndex];
            alphabet[randomIndex] = temp;
        }
        
        int columns = config.arenaColumns;
        int rows = config.arenaRows;
        
        // Calculate spacing to evenly distribute nodes
        float spacingX = config.arenaWidth / (columns + 1);
        float spacingZ = config.arenaHeight / (rows + 1);
        
        // Calculate starting position to center the grid
        float startX = -config.arenaWidth / 2f + spacingX;
        float startZ = -config.arenaHeight / 2f + spacingZ;
        
        int nodeIndex = 0;
        int positionIndex = 0;
        
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate grid position
                Vector3 gridPosition = new Vector3(
                    startX + col * spacingX,
                    0f,
                    startZ + row * spacingZ
                );
                
                // Add small random offset in random direction
                float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                float randomDistance = Random.Range(config.nodeRandomOffsetMin, config.nodeRandomOffsetMax);
                Vector3 randomOffset = new Vector3(
                    Mathf.Cos(randomAngle) * randomDistance,
                    0f,
                    Mathf.Sin(randomAngle) * randomDistance
                );
                
                Vector3 finalPosition = gridPosition + randomOffset;
                
                // Determine which letter to use
                char letterToUse;
                if (nodeIndex < 26)
                {
                    // Use the shuffled alphabet for first 26 nodes
                    letterToUse = alphabet[nodeIndex];
                }
                else
                {
                    // For the last 2 positions, pick random letters from the alphabet
                    letterToUse = alphabet[Random.Range(0, 26)];
                }
                
                GameObject nodeObj = Instantiate(letterNodePrefab, finalPosition, Quaternion.identity, arenaParent);
                nodeObj.name = "Node_" + letterToUse + "_" + positionIndex;
                
                LetterNode node = nodeObj.GetComponent<LetterNode>();
                node.Initialize(letterToUse);
                letterNodes.Add(node);
                
                nodeIndex++;
                positionIndex++;
            }
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
        
        Vector3 spawnPos = new Vector3(0, 1, -config.arenaHeight / 3f);
        GameObject playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity, actorsParent);
        playerObj.name = "Player";
        
        player = playerObj.GetComponent<PlayerController>();
        
        // Refresh the animation controller reference after swapping model
        CharacterAnimationController animController = playerObj.GetComponent<CharacterAnimationController>();
        // if (animController != null)
        // {
        //     // Force it to find the new animator
        //     animController.RefreshAnimator();
        // }
        
        // Swap character model based on selection
        SwapPlayerCharacterModel(playerObj, animController);
        
        
        
        // Use difficulty-based time limit
        float timeLimit = currentDifficulty != null ? currentDifficulty.timeLimit : config.playerStartingTime;
        player.Initialize(0, "Player", Color.green, config.playerMoveSpeed);
        player.currentTime = timeLimit;
    }
    
    void SwapPlayerCharacterModel(GameObject playerObj, CharacterAnimationController animController)
    {
        // Get selected character path
        string selectedCharacterPath = CharacterSelectionManager.GetSelectedCharacterPath();
        
        // Find the existing CharacterModel child
        Transform existingModel = playerObj.transform.Find("CharacterModel");
        if (existingModel != null)
        {
            Destroy(existingModel.gameObject);
        }
        
        // Load the selected character prefab
#if UNITY_EDITOR
        GameObject characterPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(selectedCharacterPath);
#else
        string characterName = CharacterSelectionManager.GetSelectedCharacterName();
        GameObject characterPrefab = Resources.Load<GameObject>(characterName);
#endif
        
        if (characterPrefab != null)
        {
            // Instantiate the new character model as a child
            GameObject newModel = Instantiate(characterPrefab, playerObj.transform);
            newModel.name = "CharacterModel";
            newModel.transform.localPosition = new Vector3(0, -1, 0);
            newModel.transform.localRotation = Quaternion.identity;
            newModel.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
            
            // Remove any colliders from the character model (player prefab has its own collider)
            Collider[] colliders = newModel.GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                Destroy(col);
            }
            
            // Remove CharacterAnimationController from the character model if it exists
            // (The Player parent has its own CharacterAnimationController)
            CharacterAnimationController charAnimController = newModel.GetComponent<CharacterAnimationController>();
            if (charAnimController != null)
            {
                Destroy(charAnimController);
            }
            
            // Ensure animator starts with Idle_A animation
            Animator animator = newModel.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Idle_A", 0, 0f);
                animController.AssignNewAnimator(animator);
            }
        }
        else
        {
            Debug.LogWarning("Failed to load selected character: " + selectedCharacterPath);
        }
    }
    
    void CreateBots()
    {
        Color[] botColors = { Color.red, Color.blue, Color.yellow, Color.magenta };
        
        // Use difficulty-based bot speed
        float botSpeed = currentDifficulty != null ? currentDifficulty.botSpeed : config.botMoveSpeed;
        
        for (int i = 0; i < config.botCount; i++)
        {
            float angle = (i + 1) * (360f / (config.botCount + 1)) * Mathf.Deg2Rad;
            float radius = Mathf.Max(config.arenaWidth, config.arenaHeight) / 4f;
            Vector3 spawnPos = new Vector3(
                Mathf.Cos(angle) * radius,
                1,
                Mathf.Sin(angle) * radius
            );
            
            // Use different bot prefab for each bot
            GameObject botPrefabToUse = (botPrefabs != null && botPrefabs.Length > i) 
                ? botPrefabs[i] 
                : botPrefabs[0];
            
            GameObject botObj = Instantiate(botPrefabToUse, botPositions[i], Quaternion.identity, actorsParent);
            botObj.name = "Bot_" + (i + 1);
            
            BotController bot = botObj.GetComponent<BotController>();
            bot.Initialize(i + 1, "Bot " + (i + 1), botColors[i % botColors.Length], botSpeed);
            
            // Set difficulty settings for bot
            if (currentDifficulty != null)
            {
                bot.SetDifficultySettings(currentDifficulty);
            }
            
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
            
            // Show tutorial if this is Level 1 and tutorial hasn't been completed
            if (currentLevel == 1 && !SettingsManager.GetTutorialCompleted())
            {
                uiManager.ShowTutorial();
            }
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
            // Player won - unlock next level
            SettingsManager.UnlockNextLevel();
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.PlayGameWin();
            }
            
            if (uiManager != null)
            {
                uiManager.ShowVictoryScreen(currentLevel);
            }
        }
        else
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.PlayGameLose();
            }
            
            if (uiManager != null)
            {
                uiManager.ShowLoseScreen(actor.actorName + " won!");
            }
        }
    }
    
    public void OnPlayerLost(string reason)
    {
        if (!gameActive) return;
        
        gameActive = false;
        player.isEliminated = true;
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayGameLose();
        }
        
        if (uiManager != null)
        {
            uiManager.ShowLoseScreen(reason);
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
