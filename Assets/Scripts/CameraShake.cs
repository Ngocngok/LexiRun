using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    
    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeIntensity = 0.5f;
    [SerializeField] private float shakeFrequency = 25f;
    
    private Vector3 originalPosition;
    private bool isShaking = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        originalPosition = transform.localPosition;
    }
    
    /// <summary>
    /// Triggers a camera shake effect with default settings
    /// </summary>
    public void Shake()
    {
        Shake(shakeDuration, shakeIntensity);
    }
    
    /// <summary>
    /// Triggers a camera shake effect with custom duration and intensity
    /// </summary>
    /// <param name="duration">How long the shake lasts in seconds</param>
    /// <param name="intensity">How strong the shake is</param>
    public void Shake(float duration, float intensity)
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine(duration, intensity));
        }
    }
    
    private IEnumerator ShakeCoroutine(float duration, float intensity)
    {
        isShaking = true;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            // Calculate shake offset using Perlin noise for smooth, natural movement
            float x = (Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) - 0.5f) * 2f * intensity;
            float y = (Mathf.PerlinNoise(0f, Time.time * shakeFrequency) - 0.5f) * 2f * intensity;
            
            // Apply damping over time (shake gets weaker as it ends)
            float damping = 1f - (elapsed / duration);
            x *= damping;
            y *= damping;
            
            transform.localPosition = originalPosition + new Vector3(x, y, 0f);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Reset to original position
        transform.localPosition = originalPosition;
        isShaking = false;
    }
    
    /// <summary>
    /// Stops any ongoing shake and resets camera position
    /// </summary>
    public void StopShake()
    {
        StopAllCoroutines();
        transform.localPosition = originalPosition;
        isShaking = false;
    }
    
    /// <summary>
    /// Updates the original position (useful if camera moves during gameplay)
    /// </summary>
    public void UpdateOriginalPosition()
    {
        if (!isShaking)
        {
            originalPosition = transform.localPosition;
        }
    }
}
