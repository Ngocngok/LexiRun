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

    void Start()
    {
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
        // Fall logic
        if (transform.position.y > targetY)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
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
                            ui.ShowToast("Shield up!");
                            break;
                        case BoosterType.Slow:
                            ui.ShowToast("Slowed down!");
                            break;
                        case BoosterType.SpeedUp:
                            ui.ShowToast("Haste!");
                            break;
                        case BoosterType.Trap:
                            ui.ShowToast("Trap deployed!");
                            break;
                        case BoosterType.FreezeAllOther:
                            ui.ShowToast("Freezing all opponents!");
                            break;
                            
                            
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
