using UnityEngine;

namespace LexiRun.UI
{
    /// <summary>
    /// Animation #6: Continuous rotation animation (for settings button).
    /// Rotates continuously at a slow speed.
    /// </summary>
    public class UIRotateIdle : UIAnimator
    {
        [Header("Rotation Settings")]
        [SerializeField] private float rotationSpeed = 30f; // Degrees per second
        [SerializeField] private Vector3 rotationAxis = Vector3.forward;
        
        void Update()
        {
            if (rectTransform != null)
            {
                rectTransform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
            }
        }
        
        public override void StopAnimation()
        {
            base.StopAnimation();
            enabled = false;
        }
    }
}
