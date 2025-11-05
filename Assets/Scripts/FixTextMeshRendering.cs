using UnityEngine;

/// <summary>
/// Fixes TextMesh rendering to respect depth and render behind opaque objects.
/// Attach this to any GameObject with TextMesh that has rendering issues.
/// </summary>
[RequireComponent(typeof(TextMesh))]
[RequireComponent(typeof(MeshRenderer))]
public class FixTextMeshRendering : MonoBehaviour
{
    [Header("Rendering Settings")]
    [SerializeField] private int sortingOrder = 0;
    [SerializeField] private string sortingLayerName = "Default";
    
    void Start()
    {
        FixRendering();
    }
    
    void FixRendering()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        
        if (meshRenderer != null)
        {
            // Set sorting layer and order
            meshRenderer.sortingLayerName = sortingLayerName;
            meshRenderer.sortingOrder = sortingOrder;
            
            // Ensure material respects depth
            if (meshRenderer.material != null)
            {
                // Set render queue to AlphaTest (2450) - renders after geometry but with depth test
                // This makes text render after opaque objects but still respect depth
                meshRenderer.material.renderQueue = 2450;
                
                // Enable ZTest to respect depth buffer
                if (meshRenderer.material.HasProperty("_ZTest"))
                {
                    meshRenderer.material.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
                }
            }
        }
    }
    
    #if UNITY_EDITOR
    void OnValidate()
    {
        // Apply fix in editor when values change
        if (Application.isPlaying)
        {
            FixRendering();
        }
    }
    #endif
}
