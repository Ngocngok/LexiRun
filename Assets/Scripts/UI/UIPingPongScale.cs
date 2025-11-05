using UnityEngine;
using System.Collections;

namespace LexiRun.UI
{
    /// <summary>
    /// Animation #1: Ping-pong scale animation (breathing effect).
    /// Smoothly scales between min and max values.
    /// </summary>
    public class UIPingPongScale : UIAnimator
    {
        [Header("Ping-Pong Settings")]
        [SerializeField] private float minScale = 0.95f;
        [SerializeField] private float maxScale = 1.05f;
        [SerializeField] private float speed = 1f;
        [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        public override void PlayAnimation()
        {
            StopAllCoroutines();
            StartCoroutine(PingPongScaleRoutine());
        }
        
        IEnumerator PingPongScaleRoutine()
        {
            while (true)
            {
                float t = 0f;
                
                // Scale up
                while (t < 1f)
                {
                    t += Time.deltaTime * speed;
                    float scale = Mathf.Lerp(minScale, maxScale, curve.Evaluate(t));
                    rectTransform.localScale = originalScale * scale;
                    yield return null;
                }
                
                t = 0f;
                
                // Scale down
                while (t < 1f)
                {
                    t += Time.deltaTime * speed;
                    float scale = Mathf.Lerp(maxScale, minScale, curve.Evaluate(t));
                    rectTransform.localScale = originalScale * scale;
                    yield return null;
                }
            }
        }
    }
}
