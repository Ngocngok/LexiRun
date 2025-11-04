using UnityEngine;

public class FloatingWordDisplay : MonoBehaviour
{
    public TextMesh[] letterTexts;
    public float heightOffset = 2f;
    public float letterSpacing = 0.5f;
    public int fontSize = 50;
    public Color unfilledColor = Color.white;
    public Color filledColor = Color.blue;
    
    private Transform actorTransform;
    private Camera mainCamera;
    private WordProgress wordProgress;
    private Font lilitaFont;
    
    public void Initialize(Transform actor, WordProgress progress, float height, int size, Color unfilled, Color filled)
    {
        actorTransform = actor;
        wordProgress = progress;
        heightOffset = height;
        fontSize = size;
        unfilledColor = unfilled;
        filledColor = filled;
        
        mainCamera = Camera.main;
        
        // Load LilitaOne font
        lilitaFont = Resources.Load<Font>("LilitaOne-Regular");
        if (lilitaFont == null)
        {
            // Fallback: try to load from full path
            lilitaFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        
        CreateLetterTexts();
    }
    
    void CreateLetterTexts()
    {
        // Clear existing texts
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        if (wordProgress == null || string.IsNullOrEmpty(wordProgress.currentWord))
        {
            return;
        }
        
        int wordLength = wordProgress.currentWord.Length;
        letterTexts = new TextMesh[wordLength];
        
        float totalWidth = (wordLength - 1) * letterSpacing;
        float startX = -totalWidth / 2f;
        
        for (int i = 0; i < wordLength; i++)
        {
            GameObject letterObj = new GameObject("Letter_" + i);
            letterObj.transform.SetParent(transform);
            letterObj.transform.localPosition = new Vector3(startX + i * letterSpacing, 0, 0);
            letterObj.transform.localRotation = Quaternion.identity;
            
            TextMesh textMesh = letterObj.AddComponent<TextMesh>();
            textMesh.text = "_";
            textMesh.fontSize = fontSize;
            textMesh.color = unfilledColor;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.characterSize = 0.1f;
            
            // Apply font if available
            if (lilitaFont != null)
            {
                textMesh.font = lilitaFont;
                MeshRenderer renderer = letterObj.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = lilitaFont.material;
                }
            }
            
            letterTexts[i] = textMesh;
        }
    }
    
    public void UpdateWord(WordProgress progress)
    {
        wordProgress = progress;
        
        if (letterTexts == null || letterTexts.Length != wordProgress.currentWord.Length)
        {
            CreateLetterTexts();
        }
        
        UpdateDisplay();
    }
    
    void UpdateDisplay()
    {
        if (wordProgress == null || letterTexts == null)
        {
            return;
        }
        
        for (int i = 0; i < letterTexts.Length && i < wordProgress.currentWord.Length; i++)
        {
            if (wordProgress.filledLetters[i])
            {
                letterTexts[i].text = wordProgress.currentWord[i].ToString();
                letterTexts[i].color = filledColor;
            }
            else
            {
                letterTexts[i].text = "_";
                letterTexts[i].color = unfilledColor;
            }
        }
    }
    
    void LateUpdate()
    {
        if (actorTransform != null)
        {
            // Position above actor
            transform.position = actorTransform.position + Vector3.up * heightOffset;
        }
        
        // Billboard effect - face camera
        if (mainCamera != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        }
        
        UpdateDisplay();
    }
}
