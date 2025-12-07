using UnityEngine;
using System.Collections;

namespace LexiRun.UI
{
    public class UILetterAnimation : UIAnimator
    {
        [Header("Scale Settings")]
        [SerializeField] private float minScale = 0.8f;
        [SerializeField] private float maxScale = 1.2f;
        [SerializeField] private float scaleSpeed = 5f;

        [Header("Rotation Settings")]
        [SerializeField] private float rotationAngle = 15f;
        [SerializeField] private float rotationSpeed = 5f;
        
        public override void PlayAnimation()
        {
            StopAllCoroutines();
            StartCoroutine(AnimateRoutine());
        }

        IEnumerator AnimateRoutine()
        {
            float time = 0f;
            while (true)
            {
                time += Time.deltaTime;

                // Scale
                float scalePhase = Mathf.Sin(time * scaleSpeed); // -1 to 1
                float scaleLerp = (scalePhase + 1f) / 2f; // 0 to 1
                float currentScale = Mathf.Lerp(minScale, maxScale, scaleLerp);
                rectTransform.localScale = originalScale * currentScale;

                // Rotation (Wobble)
                float rotPhase = Mathf.Sin(time * rotationSpeed); // -1 to 1
                float currentAngle = rotPhase * rotationAngle;
                rectTransform.localRotation = originalRotation * Quaternion.Euler(0, 0, currentAngle);

                yield return null;
            }
        }
    }
}
