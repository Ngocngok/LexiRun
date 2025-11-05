using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LexiRun.UI
{
    /// <summary>
    /// Animation #8: HP pulse animation (damage feedback).
    /// Pulses and shakes when HP decreases.
    /// </summary>
    public class UIHPPulse : MonoBehaviour
    {
        [Header("Pulse Settings")]
        [SerializeField] private float pulseDuration = 0.3f;
        [SerializeField] private float pulseScale = 1.3f;
        [SerializeField] private float shakeIntensity = 10f;
        [SerializeField] private Color damageColor = Color.red;
        
        [Header("References")]
        [SerializeField] private Image hpImage;
        
        private RectTransform rectTransform;
        private Vector3 originalScale;
        private Vector3 originalPosition;
        private Color originalColor;
        private PlayerController player;
        private float lastHP = -1f;
        
        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            originalScale = rectTransform.localScale;
            originalPosition = rectTransform.localPosition;
            
            if (hpImage != null)
            {
                originalColor = hpImage.color;
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
            
            // Initialize last HP
            if (lastHP < 0)
            {
                lastHP = player.currentHP;
                return;
            }
            
            // Check if HP decreased
            if (player.currentHP < lastHP)
            {
                TriggerPulse();
                lastHP = player.currentHP;
            }
        }
        
        public void TriggerPulse()
        {
            StopAllCoroutines();
            StartCoroutine(PulseRoutine());
        }
        
        IEnumerator PulseRoutine()
        {
            float t = 0f;
            
            while (t < 1f)
            {
                t += Time.deltaTime / pulseDuration;
                
                // Scale pulse
                float scale = 1f;
                if (t < 0.3f)
                {
                    scale = Mathf.Lerp(1f, pulseScale, t / 0.3f);
                }
                else
                {
                    scale = Mathf.Lerp(pulseScale, 1f, (t - 0.3f) / 0.7f);
                }
                rectTransform.localScale = originalScale * scale;
                
                // Shake
                float shake = Mathf.Sin(t * 20f) * shakeIntensity * (1f - t);
                rectTransform.localPosition = originalPosition + new Vector3(shake, 0, 0);
                
                // Color flash
                if (hpImage != null)
                {
                    hpImage.color = Color.Lerp(damageColor, originalColor, t);
                }
                
                yield return null;
            }
            
            rectTransform.localScale = originalScale;
            rectTransform.localPosition = originalPosition;
            
            if (hpImage != null)
            {
                hpImage.color = originalColor;
            }
        }
    }
}
