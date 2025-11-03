using UnityEngine;

public class BotController : ActorController
{
    public int mistakeCount = 0;
    
    private LetterNode targetNode;
    private float replanTimer;
    
    protected override void Start()
    {
        base.Start();
        replanTimer = Random.Range(0f, gameManager.config.botReplanInterval);
    }
    
    protected override void CreateFloatingWordDisplay()
    {
        base.CreateFloatingWordDisplay();
        
        if (floatingWordDisplay != null && gameManager != null)
        {
            floatingWordDisplay.Initialize(
                transform,
                wordProgress,
                gameManager.config.floatingTextHeight,
                gameManager.config.botFloatingTextSize,
                gameManager.config.unfilledLetterColor,
                gameManager.config.filledLetterColor
            );
        }
    }
    
    void Update()
    {
        if (isEliminated || !gameManager.IsGameActive())
        {
            return;
        }
        
        replanTimer -= Time.deltaTime;
        if (replanTimer <= 0 || targetNode == null || !IsTargetValid())
        {
            SelectNewTarget();
            replanTimer = gameManager.config.botReplanInterval;
        }
    }
    
    void FixedUpdate()
    {
        if (isEliminated || !gameManager.IsGameActive())
        {
            return;
        }
        
        if (targetNode != null)
        {
            Vector3 direction = (targetNode.transform.position - transform.position);
            direction.y = 0;
            Move(direction.normalized);
        }
    }
    
    private void SelectNewTarget()
    {
        if (wordProgress.currentWord == null || wordProgress.currentWord.Length == 0)
        {
            return;
        }
        
        // Find the first unfilled letter
        char targetLetter = '\0';
        for (int i = 0; i < wordProgress.currentWord.Length; i++)
        {
            if (!wordProgress.filledLetters[i])
            {
                targetLetter = wordProgress.currentWord[i];
                break;
            }
        }
        
        if (targetLetter == '\0')
        {
            return;
        }
        
        // Find the closest node with that letter
        LetterNode[] allNodes = FindObjectsByType<LetterNode>(FindObjectsSortMode.None);
        LetterNode closestNode = null;
        float closestDistance = float.MaxValue;
        
        foreach (LetterNode node in allNodes)
        {
            if (node.letter == targetLetter)
            {
                float distance = Vector3.Distance(transform.position, node.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNode = node;
                }
            }
        }
        
        targetNode = closestNode;
    }
    
    private bool IsTargetValid()
    {
        if (targetNode == null)
        {
            return false;
        }
        
        return wordProgress.IsLetterNeeded(targetNode.letter);
    }
    
    protected override void OnWrongTouch(LetterNode node)
    {
        base.OnWrongTouch(node);
        
        mistakeCount++;
        
        if (mistakeCount >= gameManager.config.botMistakeLimit)
        {
            isEliminated = true;
            gameObject.SetActive(false);
        }
    }
    
    protected override void OnWordAssigned(string word)
    {
        base.OnWordAssigned(word);
        SelectNewTarget();
    }
}
