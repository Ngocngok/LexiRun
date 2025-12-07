using UnityEngine;

public enum BoosterType
{
    Shield,
    Slow,
    SpeedUp,
    Trap,
    FreezeAllOther
}

public class Booster : MonoBehaviour
{
    private float fallSpeed = 5f;
    private float targetY = 0.5f;
    private bool hasLanded = false;
    
    // Animation
    private Vector3 originalScale;
    private float scaleSpeed = 2f;
    private float scaleAmount = 0.2f;

    void Start()
    {
        originalScale = transform.localScale;

        // Ensure collider
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = Vector3.one;
        }
        else
        {
            collider.isTrigger = true;
        }
    }

    void Update()
    {
        if (!hasLanded)
        {
            // Fall logic
            if (transform.position.y > targetY)
            {
                transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            }
            else
            {
                OnLanded();
            }
        }
        else
        {
            // Scale Loop Animation
            float scale = 1f + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
            transform.localScale = originalScale * scale;
        }
    }

    void OnLanded()
    {
        hasLanded = true;
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);

        // Spawn Land Effect
        if (GameManager.Instance != null && GameManager.Instance.config.boosterLandEffectPrefab != null)
        {
            GameObject effect = Instantiate(GameManager.Instance.config.boosterLandEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.localScale *= 4f; // 4x scale
            Destroy(effect, 2f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ActorController actor = other.GetComponent<ActorController>();
        if (actor != null)
        {
            // Generate random type
            BoosterType randomType = (BoosterType)Random.Range(0, System.Enum.GetValues(typeof(BoosterType)).Length);
            
            actor.OnBoosterCollected(randomType);
            
            // Show Toast if it's the player
            if (actor is PlayerController)
            {
                UIManager ui = FindFirstObjectByType<UIManager>();
                if (ui != null)
                {
                    switch (randomType)
                    {
                        case BoosterType.Shield:
                            ui.ShowToast("Shield up! Prevent collect wrong letter once!");
                            break;
                        case BoosterType.Slow:
                            ui.ShowToast("Slowed down! Speed reduced temporarily.");
                            break;
                        case BoosterType.SpeedUp:
                            ui.ShowToast("Haste! Speed increased temporarily.");
                            break;
                        case BoosterType.Trap:
                            ui.ShowToast("Trap deployed! You can not move for a short time.");
                            break;
                        case BoosterType.FreezeAllOther:
                            ui.ShowToast("Freezing all opponents! They won't move for a short time.");
                            break;
                            
                            
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
