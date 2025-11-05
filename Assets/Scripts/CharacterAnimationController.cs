using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator animator;
    private string currentAnimation = "";
    
    // Animation parameter names
    private const string ANIM_IDLE = "Idle_A";
    private const string ANIM_WALK = "Walk";
    
    void Start()
    {
        RefreshAnimator();
    }
    
    public void RefreshAnimator()
    {
        if(animator != null) return;
        animator = GetComponentInChildren<Animator>();
        
        if (animator != null)
        {
            Debug.Log("RefreshAnimator: Found animator on " + animator.gameObject.name);
            // Start with idle animation
            SetIdle();
        }
        else
        {
            Debug.LogWarning("RefreshAnimator: No animator found in children!");
        }
    }
    
    public void AssignNewAnimator(Animator newAnimator)
    {
        animator = newAnimator;
        if (animator != null)
        {
            Debug.Log("AssignNewAnimator: Assigned new animator on " + animator.gameObject.name);
        }
        else
        {
            Debug.LogWarning("AssignNewAnimator: New animator is null!");
        }
    }
    
    public void SetIdle()
    {
        SetAnimation(ANIM_IDLE);
    }
    
    public void SetWalk()
    {
        SetAnimation(ANIM_WALK);
    }
    
    public void RotateTowards(Vector3 direction)
    {
        if (direction.magnitude > 0.01f)
        {
            direction.y = 0; // Keep rotation on Y-axis only
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
    
    private void SetAnimation(string animationName)
    {
        if (animator == null)
        {
            Debug.LogWarning("SetAnimation: Animator is null! Cannot play " + animationName);
            return;
        }
        
        // Only play if it's a different animation
        if (currentAnimation != animationName)
        {
            Debug.Log("Playing animation: " + animationName + " on " + gameObject.name);
            animator.Play(animationName);
            currentAnimation = animationName;
        }
    }
}
