using UnityEngine;

public class Bomb : MonoBehaviour
{
    private bool hasExploded = false;

    void Start()
    {
        // Ensure we have a collider
        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = 1.0f; // Adjust as needed
        }
        else
        {
            collider.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasExploded) return;

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
        if (GameManager.Instance != null && GameManager.Instance.config.bombExplosionVFX != null)
        {
            Instantiate(GameManager.Instance.config.bombExplosionVFX, transform.position, Quaternion.identity);
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
