using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LexiRun.UI
{
    /// <summary>
    /// Animation #7: Timer flash animation (urgency indicator).
    /// Flashes red when timer is low.
    /// </summary>
    public class UITimerFlash : MonoBehaviour
    {
        [Header("Flash Settings")]
        [SerializeField] private float warningThreshold = 10f;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color warningColor = Color.red;
        [SerializeField] private float flashSpeed = 2f;
        
        [Header("References")]
        [SerializeField] private Text timerText;
        [SerializeField] private Image timerImage;
        
        private PlayerController player;
        private bool isFlashing = false;
        
        void Start()
        {
            if (timerText == null)
            {
                timerText = GetComponent<Text>();
            }
        }
        
        void Update()
        {
            // Find player if not found
            if (player == null)
            {
                player = FindObjectOfType<PlayerController>();
                if (player == null) return;
            }
            
            // Check if timer is low
            bool shouldFlash = player.currentTime <= warningThreshold && player.currentTime > 0;
            
            if (shouldFlash && !isFlashing)
            {
                StartFlashing();
            }
            else if (!shouldFlash && isFlashing)
            {
                StopFlashing();
            }
            
            // Apply flash effect
            if (isFlashing)
            {
                float t = (Mathf.Sin(Time.time * flashSpeed * Mathf.PI) + 1f) * 0.5f;
                Color flashColor = Color.Lerp(normalColor, warningColor, t);
                
                if (timerText != null)
                {
                    timerText.color = flashColor;
                }
                
                if (timerImage != null)
                {
                    timerImage.color = flashColor;
                }
            }
        }
        
        void StartFlashing()
        {
            isFlashing = true;
        }
        
        void StopFlashing()
        {
            isFlashing = false;
            
            if (timerText != null)
            {
                timerText.color = normalColor;
            }
            
            if (timerImage != null)
            {
                timerImage.color = normalColor;
            }
        }
    }
}
