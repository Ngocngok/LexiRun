using UnityEngine;

public class BotController : ActorController
{
    public int mistakeCount = 0;
    
    private LetterNode targetNode;
    private float replanTimer;
    private Vector3 smoothedAvoidanceDirection;
    
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
            
            // Check for nearby wrong nodes and avoid them
            Vector3 avoidanceDirection = GetAvoidanceDirection();
            
            // Smooth the avoidance direction to prevent jittering
            smoothedAvoidanceDirection = Vector3.Lerp(
                smoothedAvoidanceDirection, 
                avoidanceDirection, 
                Time.fixedDeltaTime * gameManager.config.botAvoidanceSmoothSpeed
            );
            
            if (smoothedAvoidanceDirection.magnitude > 0.1f)
            {
                // Blend target direction with smoothed avoidance direction
                direction = (direction.normalized + smoothedAvoidanceDirection * 1.5f).normalized;
            }
            
            // Rotate character model towards movement direction
            if (animationController != null && direction.magnitude > 0.01f)
            {
                animationController.RotateTowards(direction);
            }
            
            Move(direction.normalized);
        }
        else
        {
            // No target, set idle
            if (animationController != null)
            {
                animationController.SetIdle();
            }
        }
    }
    
    private void SelectNewTarget()
    {
        if (wordProgress.currentWord == null || wordProgress.currentWord.Length == 0)
        {
            if (animationController != null)
            {
                animationController.SetIdle();
            }
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
            if (animationController != null)
            {
                animationController.SetIdle();
            }
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
        
        // Set walk animation when we have a target
        if (targetNode != null && animationController != null)
        {
            animationController.SetWalk();
        }
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
    
    public bool ShouldAvoidNode(LetterNode node)
    {
        // If avoidance is disabled, don't avoid any nodes
        if (!gameManager.config.botAvoidWrongNodes)
        {
            return false;
        }
        
        // Bot should avoid nodes with letters they don't need
        return !wordProgress.IsLetterNeeded(node.letter);
    }
    
    private Vector3 GetAvoidanceDirection()
    {
        // If avoidance is disabled, return zero
        if (!gameManager.config.botAvoidWrongNodes)
        {
            return Vector3.zero;
        }
        
        Vector3 avoidance = Vector3.zero;
        LetterNode[] allNodes = FindObjectsByType<LetterNode>(FindObjectsSortMode.None);
        float avoidanceRadius = gameManager.config.botAvoidanceRadius;
        int avoidanceCount = 0;
        
        foreach (LetterNode node in allNodes)
        {
            // Check if this is a wrong node
            if (!wordProgress.IsLetterNeeded(node.letter))
            {
                Vector3 toNode = node.transform.position - transform.position;
                toNode.y = 0;
                float distance = toNode.magnitude;
                
                // If we're close to a wrong node, move away from it
                if (distance < avoidanceRadius && distance > 0.1f)
                {
                    // Use squared falloff for smoother transitions
                    float normalizedDistance = distance / avoidanceRadius;
                    float avoidanceStrength = (1f - normalizedDistance) * (1f - normalizedDistance);
                    
                    Vector3 awayFromNode = -toNode.normalized;
                    avoidance += awayFromNode * avoidanceStrength;
                    avoidanceCount++;
                }
            }
        }
        
        // Average the avoidance if multiple nodes are affecting the bot
        if (avoidanceCount > 0)
        {
            avoidance /= avoidanceCount;
        }
        
        // Clamp the magnitude to prevent extreme avoidance
        if (avoidance.magnitude > 1f)
        {
            avoidance = avoidance.normalized;
        }
        
        return avoidance;
    }
    
    protected override void OnWordAssigned(string word)
    {
        base.OnWordAssigned(word);
        SelectNewTarget();
    }
}
