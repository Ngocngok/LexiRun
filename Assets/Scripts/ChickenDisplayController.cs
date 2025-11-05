using UnityEngine;

/// <summary>
/// Controls the chicken character display on Win/Lose screens using RenderTexture.
/// Manages animation states for victory (Spin) and defeat (Idle + Eyes_Cry).
/// </summary>
public class ChickenDisplayController : MonoBehaviour
{
    [Header("Display Type")]
    [SerializeField] private DisplayType displayType = DisplayType.Victory;
    
    [Header("References")]
    [SerializeField] private Animator chickenAnimator;
    
    public enum DisplayType
    {
        Victory,  // Plays Spin animation
        Defeat    // Plays Idle + Eyes_Cry animations
    }
    
    void Start()
    {
        if (chickenAnimator == null)
        {
            chickenAnimator = GetComponentInChildren<Animator>();
        }
        
        if (chickenAnimator == null)
        {
            Debug.LogWarning("ChickenDisplayController: No Animator found!");
            return;
        }
        
        SetupAnimation();
    }
    
    void SetupAnimation()
    {
        switch (displayType)
        {
            case DisplayType.Victory:
                // Play Spin animation on base layer
                chickenAnimator.Play("Spin", 0);
                break;
                
            case DisplayType.Defeat:
                // Play Idle on base layer
                chickenAnimator.Play("Idle_A", 0);
                
                // Play Eyes_Cry on eyes layer (layer 1)
                chickenAnimator.Play("Eyes_Cry", 1);
                break;
        }
    }
    
    /// <summary>
    /// Manually trigger animation setup (useful for testing)
    /// </summary>
    public void RefreshAnimation()
    {
        SetupAnimation();
    }
}
