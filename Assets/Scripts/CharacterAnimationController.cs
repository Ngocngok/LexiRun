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
        animator = GetComponentInChildren<Animator>();
        
        if (animator != null)
        {
            // Start with idle animation
            SetIdle();
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
        if (animator == null) return;
        
        // Only play if it's a different animation
        if (currentAnimation != animationName)
        {
            animator.Play(animationName);
            currentAnimation = animationName;
        }
    }
}
