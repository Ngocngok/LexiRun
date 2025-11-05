using UnityEngine;
using System.Collections;

/// <summary>
/// Makes a character wander randomly between waypoint nodes in the HomeScene.
/// Walks to a random waypoint, pauses (idle), then picks another random waypoint.
/// </summary>
public class ChickenWanderer : MonoBehaviour
{
    [Header("Waypoint Settings")]
    [Tooltip("Array of waypoint transforms to move between")]
    public Transform[] waypoints;
    
    [Header("Movement Settings")]
    [Tooltip("Movement speed")]
    [SerializeField] private float moveSpeed = 2f;
    
    [Tooltip("Minimum pause duration at each waypoint")]
    [SerializeField] private float minPauseDuration = 1f;
    
    [Tooltip("Maximum pause duration at each waypoint")]
    [SerializeField] private float maxPauseDuration = 3f;
    
    [Tooltip("Distance threshold to consider waypoint reached")]
    [SerializeField] private float reachedThreshold = 0.1f;
    
    [Tooltip("Rotation speed when turning")]
    [SerializeField] private float rotationSpeed = 5f;
    
    private CharacterAnimationController animationController;
    private Transform currentTarget;
    private bool isMoving = false;
    private bool isPaused = false;
    private bool isWalkingAnimationSet = false;
    
    void Start()
    {
        animationController = GetComponentInChildren<CharacterAnimationController>();
        
        // Auto-find waypoints if not assigned
        if (waypoints == null || waypoints.Length == 0)
        {
            GameObject waypointParent = GameObject.Find("WaypointNodes");
            if (waypointParent != null)
            {
                waypoints = new Transform[waypointParent.transform.childCount];
                for (int i = 0; i < waypointParent.transform.childCount; i++)
                {
                    waypoints[i] = waypointParent.transform.GetChild(i);
                }
                Debug.Log($"ChickenWanderer: Auto-found {waypoints.Length} waypoints");
            }
        }
        
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("ChickenWanderer: No waypoints assigned!");
            enabled = false;
            return;
        }
        
        // Start at a random waypoint
        transform.position = waypoints[Random.Range(0, waypoints.Length)].position;
        
        // Start wandering
        StartCoroutine(WanderRoutine());
    }
    
    IEnumerator WanderRoutine()
    {
        while (true)
        {
            // Pick a random waypoint
            currentTarget = waypoints[Random.Range(0, waypoints.Length)];
            
            // Start moving - set animation ONCE
            isMoving = true;
            isPaused = false;
            
            if (animationController != null && !isWalkingAnimationSet)
            {
                animationController.SetWalk();
                isWalkingAnimationSet = true;
            }
            
            // Move to waypoint
            while (Vector3.Distance(transform.position, currentTarget.position) > reachedThreshold)
            {
                // Move towards target
                Vector3 direction = (currentTarget.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                
                // Rotate towards target
                if (direction.magnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                
                yield return null;
            }
            
            // Reached waypoint - pause - set animation ONCE
            isMoving = false;
            isPaused = true;
            
            if (animationController != null && isWalkingAnimationSet)
            {
                animationController.SetIdle();
                isWalkingAnimationSet = false;
            }
            
            // Wait for random duration
            float pauseDuration = Random.Range(minPauseDuration, maxPauseDuration);
            yield return new WaitForSeconds(pauseDuration);
        }
    }
    
    void OnDrawGizmos()
    {
        // Draw waypoints and connections in editor
        if (waypoints != null && waypoints.Length > 0)
        {
            Gizmos.color = Color.yellow;
            foreach (Transform waypoint in waypoints)
            {
                if (waypoint != null)
                {
                    Gizmos.DrawWireSphere(waypoint.position, 0.3f);
                }
            }
            
            // Draw line to current target
            if (Application.isPlaying && currentTarget != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, currentTarget.position);
            }
        }
    }
}
