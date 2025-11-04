using UnityEngine;

public class PlayerController : ActorController
{
    public float currentHP;
    public float currentTime;
    
    private Vector3 moveInput;
    
    protected override void Start()
    {
        base.Start();
        currentHP = gameManager.config.playerStartingHP;
        //currentTime = gameManager.config.playerStartingTime;
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
                gameManager.config.playerFloatingTextSize,
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
        
        // Update timer
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            gameManager.OnPlayerLost("Time ran out!");
        }
    }
    
    void FixedUpdate()
    {
        if (isEliminated || !gameManager.IsGameActive())
        {
            return;
        }
        
        // Rotate character model if moving
        if (animationController != null && moveInput.magnitude > 0.01f)
        {
            animationController.RotateTowards(moveInput);
        }
        
        Move(moveInput);
    }
    
    public void SetMoveInput(Vector2 input)
    {
        moveInput = new Vector3(input.x, 0, input.y);
        
        // Set animation based on input
        if (animationController != null)
        {
            if (input.magnitude > 0.1f)
            {
                animationController.SetWalk();
                animationController.RotateTowards(moveInput);
            }
            else
            {
                animationController.SetIdle();
            }
        }
    }
    
    protected override void OnWrongTouch(LetterNode node)
    {
        base.OnWrongTouch(node);
        
        // Play wrong letter sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayWrongLetter();
        }
        
        int progress = wordProgress.GetProgress();
        
        // Always deduct HP
        currentHP -= gameManager.config.hpLossAmount;
        
        // If no letters collected, also deduct time
        if (progress == 0)
        {
            currentTime -= gameManager.config.timeDeductionAtZeroProgress;
            if (currentTime <= 0)
            {
                currentTime = 0;
                gameManager.OnPlayerLost("Time ran out!");
                return;
            }
        }
        
        // Check if HP reached zero
        if (currentHP <= 0)
        {
            currentHP = 0;
            gameManager.OnPlayerLost("HP reached zero!");
        }
    }
}
