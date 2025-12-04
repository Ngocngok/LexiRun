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

        // Setup Visuals based on type
        SetupVisuals();
    }

    void SetupVisuals()
    {
        // Create a primitive child for visual representation
        GameObject visual = null;
        Color color = Color.white;

        switch (type)
        {
            case BoosterType.Shield:
                visual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                color = Color.blue;
                break;
            case BoosterType.Slow:
                visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
                color = new Color(0.6f, 0.4f, 0.2f); // Brown
                break;
            case BoosterType.SpeedUp:
                visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                color = Color.yellow;
                break;
            case BoosterType.Trap:
                visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
                color = new Color(0.5f, 0f, 0.5f); // Purple
                break;
            case BoosterType.FreezeAllOther:
                visual = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                color = Color.cyan;
                break;
        }

        if (visual != null)
        {
            visual.transform.SetParent(transform);
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localScale = Vector3.one * 0.8f;
            
            // Remove collider from visual
            Destroy(visual.GetComponent<Collider>());

            MeshRenderer renderer = visual.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
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
