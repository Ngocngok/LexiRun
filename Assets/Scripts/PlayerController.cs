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
        currentTime = gameManager.config.playerStartingTime;
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
        
        Move(moveInput);
    }
    
    public void SetMoveInput(Vector2 input)
    {
        moveInput = new Vector3(input.x, 0, input.y);
    }
    
    protected override void OnWrongTouch(LetterNode node)
    {
        base.OnWrongTouch(node);
        
        int progress = wordProgress.GetProgress();
        
        if (progress > 0)
        {
            // Remove last filled letter and decrease HP
            wordProgress.RemoveLastFilledLetter();
            currentHP -= gameManager.config.hpLossAmount;
            
            if (currentHP <= 0)
            {
                currentHP = 0;
                gameManager.OnPlayerLost("HP reached zero!");
            }
        }
        else
        {
            // Deduct time
            currentTime -= gameManager.config.timeDeductionAtZeroProgress;
            if (currentTime <= 0)
            {
                currentTime = 0;
                gameManager.OnPlayerLost("Time ran out!");
            }
        }
    }
}
