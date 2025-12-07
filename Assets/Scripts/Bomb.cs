using UnityEngine;
using System.Collections.Generic;

public class Bomb : MonoBehaviour
{
    private bool hasExploded = false;
    private bool isReady = true; // Default to true for backward compatibility or direct placement
    private SphereCollider myCollider;

    void Awake()
    {
        // Ensure we have a collider
        myCollider = GetComponent<SphereCollider>();
        if (myCollider == null)
        {
            myCollider = gameObject.AddComponent<SphereCollider>();
            myCollider.isTrigger = true;
            myCollider.radius = 1.0f; // Adjust as needed
        }
        else
        {
            myCollider.isTrigger = true;
        }
    }

    public void Initialize(Vector3 startPos, Vector3 targetPos, float duration)
    {
        isReady = false;
        if (myCollider != null) myCollider.enabled = false;
        StartCoroutine(BounceRoutine(startPos, targetPos, duration));
    }

    System.Collections.IEnumerator BounceRoutine(Vector3 start, Vector3 end, float totalDuration)
    {
        int bounces = 3;
        List<Vector3> points = new List<Vector3>();
        points.Add(start);
        
        // Calculate intermediate ground points
        for (int i = 1; i < bounces; i++)
        {
            float t = (float)i / bounces;
            Vector3 point = Vector3.Lerp(start, end, t);
            point.y = 0.5f; // Force ground level
            points.Add(point);
        }
        points.Add(end);

        float timePerBounce = totalDuration / bounces;
        Vector3 randomRotationAxis = Random.onUnitSphere;
        float rotationSpeed = 360f;

        for (int i = 0; i < bounces; i++)
        {
            Vector3 pStart = points[i];
            Vector3 pEnd = points[i+1];
            
            float elapsed = 0f;
            // Decrease height for each bounce
            // First bounce (drop) is high, subsequent ones lower
            float height = (i == 0) ? 5f : (8f / (i + 1));

            while (elapsed < timePerBounce)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / timePerBounce;
                
                Vector3 currentPos = Vector3.Lerp(pStart, pEnd, t);
                currentPos.y += 4f * height * t * (1f - t);
                
                transform.position = currentPos;
                transform.Rotate(randomRotationAxis, rotationSpeed * Time.deltaTime);
                
                yield return null;
            }
            
            // Optional: Play bounce sound here
        }

        transform.position = end;
        transform.rotation = Quaternion.identity;
        
        isReady = true;
        if (myCollider != null) myCollider.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasExploded || !isReady) return;

        ActorController actor = other.GetComponent<ActorController>();
        if (actor != null)
        {
            Explode(actor);
        }
    }

    private void Explode(ActorController victim)
    {
        hasExploded = true;

        // Apply effect to the victim
        if (victim != null)
        {
            victim.OnBombHit();
        }

        // Spawn Explosion VFX
        if (GameManager.Instance != null && GameManager.Instance.config.nukeExplosionVFX != null)
        {
            GameObject explosion = Instantiate(GameManager.Instance.config.nukeExplosionVFX, transform.position, Quaternion.identity);
            explosion.transform.localScale *= 2f; // 2x larger
        }
        else if (GameManager.Instance != null && GameManager.Instance.config.bombExplosionVFX != null)
        {
            GameObject explosion = Instantiate(GameManager.Instance.config.bombExplosionVFX, transform.position, Quaternion.identity);
            explosion.transform.localScale *= 2f; // 2x larger
        }

        // Play Explosion Sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBombExplosion();
        }

        Debug.Log($"Bomb exploded on {victim.name}!");

        // Destroy the bomb object
        Destroy(gameObject);
    }
}
