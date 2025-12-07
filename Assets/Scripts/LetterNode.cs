using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LetterNode : MonoBehaviour
{
    public char letter;
    public TextMeshPro letterText;
    public MeshRenderer nodeRenderer;
    public GameObject lockVisualPrefab;
    
    private Material nodeMaterial;
    private Color defaultColor = Color.white;
    private Dictionary<int, float> touchCooldowns = new Dictionary<int, float>();
    
    private bool isLocked = false;
    private bool isMoving = false;
    private float lockTimer = 0f;
    private GameObject lockVisual;
    private GameObject cageVisual;
    private List<ActorController> actorsInTrigger = new List<ActorController>();

    public bool IsLocked => isLocked;
    
    void Start()
    {
        if (nodeRenderer != null)
        {
            nodeMaterial = nodeRenderer.material;
        }
        
        if (lockVisualPrefab != null)
        {
            lockVisual = Instantiate(lockVisualPrefab, transform);
            lockVisual.transform.localPosition = new Vector3(0, 1.0f, 0); // Position above the node
            lockVisual.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            lockVisual.name = "LockVisual";
            lockVisual.SetActive(false);
        }
        else
        {
            // Fallback if prefab is missing
            lockVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lockVisual.transform.SetParent(transform);
            lockVisual.transform.localPosition = new Vector3(0, 1.0f, 0);
            lockVisual.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            lockVisual.name = "LockVisual";
            Destroy(lockVisual.GetComponent<Collider>());
            MeshRenderer lockRenderer = lockVisual.GetComponent<MeshRenderer>();
            if (lockRenderer != null) lockRenderer.material.color = Color.black;
            lockVisual.SetActive(false);
        }
    }
    
    void Update()
    {
        // Update lock timer
        if (isLocked)
        {
            lockTimer -= Time.deltaTime;
            // UnlockNode() is now called by the coroutine
        }

        // Update cooldowns
        List<int> keysToRemove = new List<int>();
        List<int> keys = new List<int>(touchCooldowns.Keys);
        
        foreach (int key in keys)
        {
            touchCooldowns[key] = touchCooldowns[key] - Time.deltaTime;
            if (touchCooldowns[key] <= 0)
            {
                keysToRemove.Add(key);
            }
        }
        
        foreach (int key in keysToRemove)
        {
            touchCooldowns.Remove(key);
        }
    }
    
    public void Initialize(char letter)
    {
        this.letter = char.ToUpper(letter);
        if (letterText != null)
        {
            letterText.text = this.letter.ToString();
        }
    }
    
    public bool CanTouch(int actorId)
    {
        if (isLocked || isMoving) return false;
        return !touchCooldowns.ContainsKey(actorId) || touchCooldowns[actorId] <= 0;
    }
    
    public void LockNode(float duration)
    {
        if (isLocked) return;
        
        isLocked = true;
        lockTimer = duration;
        StartCoroutine(LockSequence(duration));
    }

    private IEnumerator LockSequence(float duration)
    {
        // 1. Spawn Cage above
        if (GameManager.Instance != null && GameManager.Instance.config.cagePrefab != null)
        {
            cageVisual = Instantiate(GameManager.Instance.config.cagePrefab, transform);
            cageVisual.transform.localPosition = new Vector3(0, 5f, 0); // Start high
        }

        // 2. Move Cage down (0.5s)
        if (cageVisual != null)
        {
            float elapsed = 0f;
            Vector3 startPos = cageVisual.transform.localPosition;
            Vector3 endPos = Vector3.zero; // Assuming pivot is at bottom or center aligns with node
            
            while (elapsed < 0.5f)
            {
                elapsed += Time.deltaTime;
                cageVisual.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / 0.5f);
                yield return null;
            }
            cageVisual.transform.localPosition = endPos;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        // 3. Spawn Lock above cage
        if (lockVisual != null)
        {
            lockVisual.SetActive(true);
            // Ensure lock is visible above cage if needed, but lockVisual is already positioned at (0, 1, 0)
        }

        // 4. Wait for remaining duration
        // Total duration is passed, we spent 0.5s moving down.
        float remainingTime = duration - 0.5f;
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        // 5. Despawn Lock
        if (lockVisual != null)
        {
            lockVisual.SetActive(false);
        }

        // 6. Move Cage up
        if (cageVisual != null)
        {
            float elapsed = 0f;
            Vector3 startPos = cageVisual.transform.localPosition;
            Vector3 endPos = new Vector3(0, 5f, 0);
            
            while (elapsed < 0.5f)
            {
                elapsed += Time.deltaTime;
                cageVisual.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / 0.5f);
                yield return null;
            }
        }

        // 7. Destroy Cage
        if (cageVisual != null)
        {
            Destroy(cageVisual);
            cageVisual = null;
        }

        UnlockNode();
    }
    
    private void UnlockNode()
    {
        isLocked = false;
        // Visuals handled in coroutine

        // Check for actors already in trigger
        foreach (var actor in new List<ActorController>(actorsInTrigger))
        {
            if (actor == null) continue;

            // For bots, check if they should avoid this node
            BotController bot = actor as BotController;
            if (bot != null && bot.ShouldAvoidNode(this))
            {
                continue;
            }
            
            actor.OnNodeTouched(this);
        }
    }
    
    public void SetTouchCooldown(int actorId, float cooldown)
    {
        touchCooldowns[actorId] = cooldown;
    }
    
    public void SetLastTouchedColor(Color color)
    {
        if (nodeMaterial != null)
        {
            nodeMaterial.color = color;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        ActorController actor = other.GetComponent<ActorController>();
        if (actor != null)
        {
            if (!actorsInTrigger.Contains(actor))
            {
                actorsInTrigger.Add(actor);
            }

            // For bots, check if they should avoid this node
            BotController bot = actor as BotController;
            if (bot != null && bot.ShouldAvoidNode(this))
            {
                return; // Don't trigger touch for bots on wrong nodes
            }
            
            actor.OnNodeTouched(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        ActorController actor = other.GetComponent<ActorController>();
        if (actor != null)
        {
            if (actorsInTrigger.Contains(actor))
            {
                actorsInTrigger.Remove(actor);
            }
        }
    }

    public void JumpAndRollTo(Vector3 targetPos, float duration)
    {
        StartCoroutine(JumpAndRollRoutine(targetPos, duration));
    }

    private IEnumerator JumpAndRollRoutine(Vector3 targetPos, float duration)
    {
        isMoving = true;
        
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        
        // Calculate rotation axis (perpendicular to movement)
        Vector3 direction = (targetPos - startPos).normalized;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
        if (rotationAxis == Vector3.zero) rotationAxis = Vector3.right; // Fallback

        float elapsed = 0f;
        float jumpHeight = 2.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Parabolic movement
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
            currentPos.y += 4f * jumpHeight * t * (1f - t); // Parabola
            transform.position = currentPos;

            // Roll (360 degrees)
            float angle = Mathf.Lerp(0f, 360f, t);
            transform.rotation = Quaternion.AngleAxis(angle, rotationAxis) * startRot;

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = startRot; // Reset rotation to original
        
        isMoving = false;
    }
}
