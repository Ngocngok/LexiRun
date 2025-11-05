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
    
    void Start()
    {
        if (nodeRenderer != null)
        {
            nodeMaterial = nodeRenderer.material;
        }
    }
    
    void Update()
    {
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
        return !touchCooldowns.ContainsKey(actorId) || touchCooldowns[actorId] <= 0;
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
            // For bots, check if they should avoid this node
            BotController bot = actor as BotController;
            if (bot != null && bot.ShouldAvoidNode(this))
            {
                return; // Don't trigger touch for bots on wrong nodes
            }
            
            actor.OnNodeTouched(this);
        }
    }
}
