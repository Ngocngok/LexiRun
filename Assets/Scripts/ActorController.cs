using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    public int actorId;
    public string actorName;
    public Color actorColor = Color.white;
    public WordProgress wordProgress = new WordProgress();
    public int completedWords = 0;
    public bool isEliminated = false;
    
    protected GameManager gameManager;
    protected Rigidbody rb;
    protected float moveSpeed;
    protected float originalMoveSpeed;
    protected FloatingWordDisplay floatingWordDisplay;
    protected CharacterAnimationController animationController;

    protected float speedModifierTimer = 0f;
    protected bool isSpeedModified = false;
    protected const float SPEED_REDUCTION_DURATION = 3f;
    protected const float SPEED_REDUCTION_MULTIPLIER = 0.5f;

    protected bool hasShield = false;
    protected GameObject currentShieldVisual;
    protected bool isFrozen = false;
    protected float freezeTimer = 0f;
    
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameManager.Instance;
        animationController = GetComponent<CharacterAnimationController>();
        
        // Set actor color
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = actorColor;
        }
        
        // Create floating word display
        CreateFloatingWordDisplay();
    }
    
    public virtual void Initialize(int id, string name, Color color, float speed)
    {
        actorId = id;
        actorName = name;
        //actorColor = color;
        moveSpeed = speed;
        originalMoveSpeed = speed;
        
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = actorColor;
        }
    }
    
    protected virtual void CreateFloatingWordDisplay()
    {
        GameObject floatingTextObj = new GameObject("FloatingWordDisplay");
        floatingTextObj.transform.SetParent(transform);
        floatingTextObj.transform.localPosition = Vector3.zero;
        
        floatingWordDisplay = floatingTextObj.AddComponent<FloatingWordDisplay>();
    }
    
    public void AssignWord(string word)
    {
        wordProgress.SetWord(word);
        
        if (floatingWordDisplay != null)
        {
            floatingWordDisplay.UpdateWord(wordProgress);
        }
        
        OnWordAssigned(word);
    }
    
    protected virtual void OnWordAssigned(string word)
    {
        // Override in derived classes
    }
    
    public void OnNodeTouched(LetterNode node)
    {
        if (isEliminated || gameManager == null || !gameManager.IsGameActive())
        {
            return;
        }
        
        if (!node.CanTouch(actorId))
        {
            return;
        }
        
        node.SetTouchCooldown(actorId, gameManager.config.touchCooldown);
        node.SetLastTouchedColor(actorColor);
        
        bool isNeeded = wordProgress.IsLetterNeeded(node.letter);
        
        if (isNeeded)
        {
            OnCorrectTouch(node);
        }
        else
        {
            OnWrongTouch(node);
        }
    }
    
    protected virtual void OnCorrectTouch(LetterNode node)
    {
        // Lock the node for 4 seconds
        node.LockNode(4.0f);

        wordProgress.FillLetter(node.letter);
        
        if (floatingWordDisplay != null)
        {
            floatingWordDisplay.UpdateWord(wordProgress);
        }
        
        // Play correct letter sound only for player
        if (AudioManager.Instance != null && this is PlayerController)
        {
            AudioManager.Instance.PlayCorrectLetter();
        }
        
        if (wordProgress.IsComplete())
        {
            OnWordCompleted();
        }
    }
    
    protected virtual void OnWrongTouch(LetterNode node)
    {
        // Base implementation handles shield logic
        // Derived classes should check hasShield before applying penalties
        // But since base.OnWrongTouch() is called first, we need a way to signal if shield was used.
        // However, we can't change the return type easily without breaking everything.
        
        // Instead, we'll handle the shield consumption here, but derived classes need to know.
        // Actually, the derived classes call base.OnWrongTouch(node).
        // If I consume the shield here, the derived class continues execution.
        
        // So, I will NOT consume the shield here if I want derived classes to check it.
        // OR, I consume it here, but derived classes check if it WAS active? No.
        
        // Let's change the design slightly:
        // Derived classes are responsible for checking shield.
        // Base class provides a helper method or property?
        
        // Actually, let's just keep the logic here but make sure derived classes respect it.
        // The issue is that base.OnWrongTouch is void.
        
        if (hasShield)
        {
            hasShield = false;
            if (currentShieldVisual != null)
            {
                Destroy(currentShieldVisual);
            }
            Debug.Log($"{actorName} used Shield!");
            return;
        }

        ApplySpeedPenalty();
    }
    
    // Helper to check if shield was just consumed or is active
    protected bool CheckAndConsumeShield()
    {
        if (hasShield)
        {
            hasShield = false;
            if (currentShieldVisual != null)
            {
                Destroy(currentShieldVisual);
            }
            Debug.Log($"{actorName} used Shield!");
            return true;
        }
        return false;
    }

    public virtual void OnBombHit()
    {
        // Only play effects for player
        if (this is PlayerController)
        {
            // Trigger camera shake effect
            if (CameraShake.Instance != null)
            {
                CameraShake.Instance.Shake();
            }

            // Trigger vibration for bomb hit
            if (VibrationManager.Instance != null)
            {
                VibrationManager.Instance.VibrateWrongLetter();
            }
        }

        // If character has letters, remove one random letter
        if (wordProgress.GetProgress() > 0)
        {
            wordProgress.RemoveRandomFilledLetter();
            
            if (floatingWordDisplay != null)
            {
                floatingWordDisplay.UpdateWord(wordProgress);
            }
            
            // Play sound/VFX if available (Only for player)
            if (this is PlayerController && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayWrongLetter(); // Reuse wrong letter sound for now
            }
        }
    }

    public void OnBoosterCollected(BoosterType type)
    {
        Debug.Log($"{actorName} collected {type}");
        float duration = gameManager.config.boosterDuration;

        // Sound and Vibration Logic (Only for player)
        if (this is PlayerController)
        {
            bool isPositive = (type == BoosterType.Shield || type == BoosterType.SpeedUp || type == BoosterType.FreezeAllOther);
            
            if (AudioManager.Instance != null)
            {
                if (isPositive) AudioManager.Instance.PlayPowerUp();
                else AudioManager.Instance.PlayPowerDown();
            }

            // Trigger vibration for booster collection
            if (VibrationManager.Instance != null)
            {
                if (isPositive) VibrationManager.Instance.VibrateCorrectLetter();
                else VibrationManager.Instance.VibrateWrongLetter();
            }
        }

        switch (type)
        {
            case BoosterType.Shield:
                hasShield = true;
                if (gameManager.config.shieldVisualPrefab != null)
                {
                    if (currentShieldVisual != null) Destroy(currentShieldVisual);
                    currentShieldVisual = Instantiate(gameManager.config.shieldVisualPrefab, transform);
                    currentShieldVisual.transform.localPosition = Vector3.zero;
                }
                break;
            case BoosterType.Slow:
                ApplySpeedModifier(0.5f, duration);
                break;
            case BoosterType.SpeedUp:
                ApplySpeedModifier(1.5f, duration);
                break;
            case BoosterType.Trap:
                ApplyFreeze(duration);
                break;
            case BoosterType.FreezeAllOther:
                gameManager.FreezeAllActorsExcept(this, duration);
                break;
        }
    }

    protected void ApplySpeedPenalty()
    {
        ApplySpeedModifier(SPEED_REDUCTION_MULTIPLIER, SPEED_REDUCTION_DURATION);
    }

    protected void ApplySpeedModifier(float multiplier, float duration)
    {
        if (!isSpeedModified)
        {
            // Ensure we have the correct original speed
            if (moveSpeed > originalMoveSpeed * 0.8f && moveSpeed < originalMoveSpeed * 1.2f) 
            {
                originalMoveSpeed = moveSpeed;
            }
        }
        
        moveSpeed = originalMoveSpeed * multiplier;
        isSpeedModified = true;
        speedModifierTimer = duration;
    }

    public void ApplyFreeze(float duration)
    {
        isFrozen = true;
        freezeTimer = duration;
        // Stop movement immediately
        if (rb != null) rb.linearVelocity = Vector3.zero;
    }

    protected void HandleSpeedReduction()
    {
        // Handle Freeze
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0)
            {
                isFrozen = false;
            }
            return; // Skip speed modification logic if frozen
        }

        // Handle Speed Modifier
        if (isSpeedModified)
        {
            speedModifierTimer -= Time.deltaTime;
            if (speedModifierTimer <= 0)
            {
                moveSpeed = originalMoveSpeed;
                isSpeedModified = false;
            }
        }
    }
    
    protected virtual void OnWordCompleted()
    {
        completedWords++;
        
        // Play word complete sound only for player
        if (AudioManager.Instance != null && this is PlayerController)
        {
            AudioManager.Instance.PlayWordComplete();
        }
        
        int targetWords = gameManager.GetWordsToWin();
        if (completedWords >= targetWords)
        {
            gameManager.OnActorWon(this);
        }
        else
        {
            gameManager.AssignNewWord(this);
        }
    }
    
    protected void Move(Vector3 direction)
    {
        if (isFrozen) return;

        if (rb != null)
        {
            if (direction.magnitude > 0.1f)
            {
                Vector3 movement = direction.normalized * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + movement);
                
                // Rotate to face movement direction
                if (movement.magnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(movement);
                    rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f);
                }
            }
            else
            {
                // Stop the rigidbody when there's no input to prevent sliding
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        ActorController otherActor = collision.gameObject.GetComponent<ActorController>();
        if (otherActor != null && rb != null)
        {
            Vector3 pushDirection = (transform.position - otherActor.transform.position).normalized;
            pushDirection.y = 0; // Keep push horizontal
            
            float pushForce = gameManager != null ? gameManager.config.characterPushForce : 5f;
            
            // Apply impulse force
            rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}
