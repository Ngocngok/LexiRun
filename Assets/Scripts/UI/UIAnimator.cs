using UnityEngine;
using System.Collections;

namespace LexiRun.UI
{
    /// <summary>
    /// Base class for UI animations. Provides common animation utilities.
    /// </summary>
    public class UIAnimator : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] protected bool playOnEnable = true;
        [SerializeField] protected float delay = 0f;
        
        protected RectTransform rectTransform;
        protected Vector3 originalScale;
        protected Vector3 originalPosition;
        protected Quaternion originalRotation;
        
        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                originalScale = rectTransform.localScale;
                originalPosition = rectTransform.localPosition;
                originalRotation = rectTransform.localRotation;
            }
        }
        
        protected virtual void OnEnable()
        {
            if (playOnEnable)
            {
                if (delay > 0)
                {
                    StartCoroutine(DelayedPlay());
                }
                else
                {
                    PlayAnimation();
                }
            }
        }
        
        protected virtual void OnDisable()
        {
            StopAllCoroutines();
            ResetToOriginal();
        }
        
        IEnumerator DelayedPlay()
        {
            yield return new WaitForSeconds(delay);
            PlayAnimation();
        }
        
        public virtual void PlayAnimation()
        {
            // Override in derived classes
        }
        
        public virtual void StopAnimation()
        {
            StopAllCoroutines();
            ResetToOriginal();
        }
        
        protected virtual void ResetToOriginal()
        {
            if (rectTransform != null)
            {
                rectTransform.localScale = originalScale;
                rectTransform.localPosition = originalPosition;
                rectTransform.localRotation = originalRotation;
            }
        }
    }
}
