using UnityEngine;
using System.Collections;

namespace LexiRun.UI
{
    /// <summary>
    /// Animation #3: Bounce in animation (elastic pop-in effect).
    /// Scales from 0 to 1 with overshoot bounce.
    /// </summary>
    public class UIBounceIn : UIAnimator
    {
        [Header("Bounce Settings")]
        [SerializeField] private float bounceDuration = 0.5f;
        [SerializeField] private float overshoot = 1.2f;
        [SerializeField] private AnimationCurve bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        protected override void Awake()
        {
            base.Awake();
            // Start at zero scale
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.zero;
            }
        }
        
        public override void PlayAnimation()
        {
            StopAllCoroutines();
            StartCoroutine(BounceInRoutine());
        }
        
        IEnumerator BounceInRoutine()
        {
            float t = 0f;
            
            while (t < 1f)
            {
                t += Time.deltaTime / bounceDuration;
                float curveValue = bounceCurve.Evaluate(t);
                
                // Add overshoot effect
                float scale;
                if (t < 0.6f)
                {
                    scale = Mathf.Lerp(0f, overshoot, curveValue);
                }
                else
                {
                    scale = Mathf.Lerp(overshoot, 1f, (t - 0.6f) / 0.4f);
                }
                
                rectTransform.localScale = originalScale * scale;
                yield return null;
            }
            
            rectTransform.localScale = originalScale;
        }
    }
}
