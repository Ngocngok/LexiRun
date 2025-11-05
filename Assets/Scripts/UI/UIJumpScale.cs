using UnityEngine;
using System.Collections;

namespace LexiRun.UI
{
    /// <summary>
    /// Animation #2: Jump scale animation (attention-grabbing bounce).
    /// Scales up and down in a bouncy motion.
    /// </summary>
    public class UIJumpScale : UIAnimator
    {
        [Header("Jump Settings")]
        [SerializeField] private float jumpScale = 1.2f;
        [SerializeField] private float jumpDuration = 0.3f;
        [SerializeField] private float pauseDuration = 1f;
        [SerializeField] private AnimationCurve jumpCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        public override void PlayAnimation()
        {
            StopAllCoroutines();
            StartCoroutine(JumpScaleRoutine());
        }
        
        IEnumerator JumpScaleRoutine()
        {
            while (true)
            {
                // Jump up
                yield return StartCoroutine(ScaleToRoutine(jumpScale, jumpDuration * 0.4f));
                
                // Jump down
                yield return StartCoroutine(ScaleToRoutine(1f, jumpDuration * 0.6f));
                
                // Pause
                yield return new WaitForSeconds(pauseDuration);
            }
        }
        
        IEnumerator ScaleToRoutine(float targetScale, float duration)
        {
            Vector3 startScale = rectTransform.localScale;
            Vector3 endScale = originalScale * targetScale;
            float t = 0f;
            
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                rectTransform.localScale = Vector3.Lerp(startScale, endScale, jumpCurve.Evaluate(t));
                yield return null;
            }
            
            rectTransform.localScale = endScale;
        }
    }
}
