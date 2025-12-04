using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LetterNode : MonoBehaviour
{
    public char letter;
    public TextMeshPro letterText;
    public MeshRenderer nodeRenderer;
    
    private Material nodeMaterial;
    private Color defaultColor = Color.white;
    private Dictionary<int, float> touchCooldowns = new Dictionary<int, float>();
    
    private bool isLocked = false;
    private float lockTimer = 0f;
    private GameObject lockVisual;
    private List<ActorController> actorsInTrigger = new List<ActorController>();

    public bool IsLocked => isLocked;
    
    void Start()
    {
        if (nodeRenderer != null)
        {
            nodeMaterial = nodeRenderer.material;
        }
        
        // Create lock visual (Cube)
        lockVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lockVisual.transform.SetParent(transform);
        lockVisual.transform.localPosition = new Vector3(0, 1.0f, 0); // Position above the node
        lockVisual.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        lockVisual.name = "LockVisual";
        
        // Remove collider from lock visual so it doesn't interfere with physics
        Destroy(lockVisual.GetComponent<Collider>());
        
        // Set color to black or something distinct
        MeshRenderer lockRenderer = lockVisual.GetComponent<MeshRenderer>();
        if (lockRenderer != null)
        {
            lockRenderer.material.color = Color.black;
        }
        
        lockVisual.SetActive(false);
    }
    
    void Update()
    {
        // Update lock timer
        if (isLocked)
        {
            lockTimer -= Time.deltaTime;
            if (lockTimer <= 0)
            {
                UnlockNode();
            }
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
        if (isLocked) return false;
        return !touchCooldowns.ContainsKey(actorId) || touchCooldowns[actorId] <= 0;
    }
    
    public void LockNode(float duration)
    {
        isLocked = true;
        lockTimer = duration;
        if (lockVisual != null)
        {
            lockVisual.SetActive(true);
        }
    }
    
    private void UnlockNode()
    {
        isLocked = false;
        if (lockVisual != null)
        {
            lockVisual.SetActive(false);
        }

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
}
