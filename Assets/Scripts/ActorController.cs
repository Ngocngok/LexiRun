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
    protected FloatingWordDisplay floatingWordDisplay;
    protected CharacterAnimationController animationController;
    
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameManager.Instance;
        animationController = GetComponentInChildren<CharacterAnimationController>();
        
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
        actorColor = color;
        moveSpeed = speed;
        
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
        wordProgress.FillLetter(node.letter);
        
        if (floatingWordDisplay != null)
        {
            floatingWordDisplay.UpdateWord(wordProgress);
        }
        
        if (wordProgress.IsComplete())
        {
            OnWordCompleted();
        }
    }
    
    protected virtual void OnWrongTouch(LetterNode node)
    {
        // Override in derived classes
    }
    
    protected virtual void OnWordCompleted()
    {
        completedWords++;
        
        if (completedWords >= gameManager.config.wordsToWin)
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
        if (rb != null && direction.magnitude > 0.1f)
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
    }
}
