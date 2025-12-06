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
    public BoosterType type;

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

    void OnTriggerEnter(Collider other)
    {
        ActorController actor = other.GetComponent<ActorController>();
        if (actor != null)
        {
            actor.OnBoosterCollected(type);
            Destroy(gameObject);
        }
    }
}
