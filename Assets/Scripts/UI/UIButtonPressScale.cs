using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace LexiRun.UI
{
    /// <summary>
    /// Animation #5: Button press scale animation (tactile feedback).
    /// Scales down when pressed, bounces back when released.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class UIButtonPressScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Press Settings")]
        [SerializeField] private float pressScale = 0.9f;
        [SerializeField] private float pressDuration = 0.1f;
        [SerializeField] private float releaseDuration = 0.15f;
        [SerializeField] private AnimationCurve pressCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        private RectTransform rectTransform;
        private Vector3 originalScale;
        private Coroutine currentAnimation;
        
        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            originalScale = rectTransform.localScale;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
            }
            currentAnimation = StartCoroutine(ScaleToRoutine(pressScale, pressDuration));
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
            }
            currentAnimation = StartCoroutine(ScaleToRoutine(1f, releaseDuration));
        }
        
        IEnumerator ScaleToRoutine(float targetScale, float duration)
        {
            Vector3 startScale = rectTransform.localScale;
            Vector3 endScale = originalScale * targetScale;
            float t = 0f;
            
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                rectTransform.localScale = Vector3.Lerp(startScale, endScale, pressCurve.Evaluate(t));
                yield return null;
            }
            
            rectTransform.localScale = endScale;
        }
    }
}
