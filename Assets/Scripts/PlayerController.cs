using MoreMountains.NiceVibrations;
using UnityEngine;

public class PlayerController : ActorController
{
    public float currentHP;
    public float currentTime;
    public ParticleSystem collectLetterFX;
    public ParticleSystem collectWordFX;
    
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
                gameManager.config.playerFilledLetterColor
            );
        }
    }
    
    void Update()
    {
        
        rb.linearVelocity = Vector3.zero;
        
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

    protected override void OnWordCompleted()
    {
        base.OnWordCompleted();
        collectWordFX.Play(true);
        MMVibrationManager.Vibrate();
        // Trigger strong vibration for word completion
        if (VibrationManager.Instance != null)
        {
            VibrationManager.Instance.VibrateWordComplete();
        }
    }

    protected override void OnCorrectTouch(LetterNode node)
    {
        base.OnCorrectTouch(node);
        collectLetterFX.Play(true);
        MMVibrationManager.Vibrate();
        // Trigger light vibration for correct letter
        if (VibrationManager.Instance != null)
        {
            VibrationManager.Instance.VibrateCorrectLetter();
        }
    }

    protected override void OnWrongTouch(LetterNode node)
    {
        base.OnWrongTouch(node);
        
        // Trigger camera shake effect
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake();
        }
        MMVibrationManager.Vibrate();
        // Trigger strong vibration for wrong letter
        if (VibrationManager.Instance != null)
        {
            VibrationManager.Instance.VibrateWrongLetter();
        }
        
        // Play wrong letter sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayWrongLetter();
        }
        
        int progress = wordProgress.GetProgress();
        
        // Always deduct HP
        currentHP -= gameManager.config.hpLossAmount;
        
        // If letters collected > 0, randomly remove one letter
        if (progress > 0)
        {
            wordProgress.RemoveRandomFilledLetter();
            
            // Update floating word display
            if (floatingWordDisplay != null)
            {
                floatingWordDisplay.UpdateWord(wordProgress);
            }
        }
        // If no letters collected, deduct time instead
        else if (progress == 0)
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
