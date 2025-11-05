using UnityEngine;
using System.Collections;

namespace LexiRun.UI
{
    /// <summary>
    /// Animation #4: Shake in animation (shaking entrance).
    /// Fades in while shaking horizontally.
    /// </summary>
    public class UIShakeIn : UIAnimator
    {
        [Header("Shake Settings")]
        [SerializeField] private float shakeDuration = 0.5f;
        [SerializeField] private float shakeIntensity = 20f;
        [SerializeField] private int shakeCount = 5;
        
        private CanvasGroup canvasGroup;
        
        protected override void Awake()
        {
            base.Awake();
            
            // Add CanvasGroup if not present
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            
            // Start invisible
            canvasGroup.alpha = 0f;
        }
        
        public override void PlayAnimation()
        {
            StopAllCoroutines();
            StartCoroutine(ShakeInRoutine());
        }
        
        IEnumerator ShakeInRoutine()
        {
            float t = 0f;
            Vector3 startPos = originalPosition;
            
            while (t < 1f)
            {
                t += Time.deltaTime / shakeDuration;
                
                // Fade in
                canvasGroup.alpha = t;
                
                // Shake horizontally (decreasing intensity)
                float shakeAmount = shakeIntensity * (1f - t);
                float shake = Mathf.Sin(t * shakeCount * Mathf.PI * 2) * shakeAmount;
                rectTransform.localPosition = startPos + new Vector3(shake, 0, 0);
                
                yield return null;
            }
            
            canvasGroup.alpha = 1f;
            rectTransform.localPosition = originalPosition;
        }
        
        protected override void ResetToOriginal()
        {
            base.ResetToOriginal();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
        }
    }
}
