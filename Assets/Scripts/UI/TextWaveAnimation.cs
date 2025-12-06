using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TextWaveAnimation : MonoBehaviour
{
    public float waveSpeed = 5f;
    public float waveHeight = 10f;
    public float scaleMultiplier = 1.2f;
    
    private Text uiText;
    private TextMeshProUGUI tmpText;
    private string originalText;
    private List<GameObject> charObjects = new List<GameObject>();
    
    void Start()
    {
        uiText = GetComponent<Text>();
        tmpText = GetComponent<TextMeshProUGUI>();
        
        if (uiText != null)
        {
            originalText = uiText.text;
            // For standard UI Text, we can't easily animate individual characters without splitting them into separate objects.
            // So we'll split them.
            SplitText();
        }
        else if (tmpText != null)
        {
            // TMP has built-in vertex manipulation, but for simplicity and consistency with the request "character by character bigger then smaller",
            // we'll stick to the splitting method or use TMP's animator if available.
            // Let's assume standard UI Text for now based on the provided context.
        }
    }
    
    void SplitText()
    {
        if (uiText == null) return;
        
        uiText.enabled = false; // Hide original
        
        char[] chars = originalText.ToCharArray();
        float totalWidth = 0;
        
        // Calculate total width to center (simplified)
        // A better approach is to use a HorizontalLayoutGroup
        
        GameObject container = new GameObject("TextContainer");
        container.transform.SetParent(transform);
        container.transform.localPosition = Vector3.zero;
        container.transform.localScale = Vector3.one;
        
        HorizontalLayoutGroup layout = container.AddComponent<HorizontalLayoutGroup>();
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.childAlignment = TextAnchor.MiddleCenter;
        
        // Copy RectTransform properties
        RectTransform containerRect = container.GetComponent<RectTransform>();
        RectTransform originalRect = GetComponent<RectTransform>();
        containerRect.sizeDelta = originalRect.sizeDelta;
        containerRect.anchoredPosition = Vector2.zero;

        foreach (char c in chars)
        {
            GameObject charObj = new GameObject("Char_" + c);
            charObj.transform.SetParent(container.transform);
            charObj.transform.localScale = Vector3.one;
            
            Text t = charObj.AddComponent<Text>();
            t.text = c.ToString();
            t.font = uiText.font;
            t.fontSize = uiText.fontSize;
            t.color = uiText.color;
            t.alignment = TextAnchor.MiddleCenter;
            
            charObjects.Add(charObj);
        }
    }
    
    void Update()
    {
        if (charObjects.Count == 0) return;
        
        float time = Time.unscaledTime * waveSpeed;
        
        for (int i = 0; i < charObjects.Count; i++)
        {
            GameObject charObj = charObjects[i];
            float offset = i * 0.5f;
            
            // Wave calculation
            float wave = Mathf.Sin(time + offset);
            
            // Scale effect: 1.0 to scaleMultiplier
            float scale = 1f + (wave + 1f) * 0.5f * (scaleMultiplier - 1f);
            
            charObj.transform.localScale = Vector3.one * scale;
        }
    }
}
