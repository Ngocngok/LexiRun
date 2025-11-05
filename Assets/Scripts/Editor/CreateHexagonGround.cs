using UnityEngine;
using UnityEditor;

public class CreateHexagonGround : MonoBehaviour
{
    public static string Execute()
    {
        // Load the hex model
        GameObject hexPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Model/hex_forest.fbx");
        if (hexPrefab == null)
        {
            return "hex_forest.fbx not found at Assets/Model/hex_forest.fbx";
        }
        
        // Find Ground object
        GameObject ground = GameObject.Find("Ground");
        if (ground == null)
        {
            return "Ground object not found!";
        }
        
        // Clear existing children
        while (ground.transform.childCount > 0)
        {
            DestroyImmediate(ground.transform.GetChild(0).gameObject);
        }
        
        // First, instantiate one hex to measure its size
        GameObject tempHex = PrefabUtility.InstantiatePrefab(hexPrefab) as GameObject;
        
        // Get the bounds to calculate hex dimensions
        Renderer renderer = tempHex.GetComponentInChildren<Renderer>();
        Bounds bounds = renderer != null ? renderer.bounds : new Bounds(Vector3.zero, Vector3.one);
        
        // For a flat-topped hexagon:
        // Width (point to point horizontally) = size
        // Height (flat to flat vertically) = size * sqrt(3)/2
        float hexWidth = bounds.size.x;
        float hexHeight = bounds.size.z;
        
        DestroyImmediate(tempHex);
        
        // Arena dimensions (from GameConfig default: 30 wide x 40 tall)
        float arenaWidth = 30f;
        float arenaHeight = 40f;
        
        // Calculate required dimensions (1.5x arena size)
        float requiredWidth = arenaWidth * 1.5f;  // 45 units
        float requiredHeight = arenaHeight * 1.5f; // 60 units
        
        // Calculate how many hexagons we need
        // For honeycomb pattern with flat-topped hexagons:
        // Horizontal spacing = hexWidth * 0.75 (3/4 of width)
        // Vertical spacing = hexHeight (full height)
        
        float horizontalSpacing = hexWidth * 0.75f;
        float verticalSpacing = hexHeight;
        
        int cols = Mathf.CeilToInt(requiredWidth / horizontalSpacing) + 2;
        int rows = Mathf.CeilToInt(requiredHeight / verticalSpacing) + 2;
        
        // Make sure we have at least 20x20
        cols = Mathf.Max(cols, 20);
        rows = Mathf.Max(rows, 20);
        
        // Calculate starting position to center the grid
        float startX = -(cols * horizontalSpacing) / 2f;
        float startZ = -(rows * verticalSpacing) / 2f;
        
        int hexCount = 0;
        
        // Create honeycomb pattern
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // Calculate position
                float x = startX + (col * horizontalSpacing);
                float z = startZ + (row * verticalSpacing);
                
                // Offset every other row for honeycomb pattern
                if (row % 2 == 1)
                {
                    x += horizontalSpacing / 2f;
                }
                
                Vector3 position = new Vector3(x, 0, z);
                
                // Instantiate hex
                GameObject hex = PrefabUtility.InstantiatePrefab(hexPrefab, ground.transform) as GameObject;
                hex.transform.position = position;
                hex.transform.rotation = Quaternion.identity; // Flat-topped (default orientation)
                hex.name = "Hex_" + row + "_" + col;
                
                hexCount++;
            }
        }
        
        return $"Successfully created {hexCount} hexagons ({rows}x{cols}) in honeycomb pattern!\n" +
               $"Hex dimensions: {hexWidth:F2} x {hexHeight:F2}\n" +
               $"Coverage area: {cols * horizontalSpacing:F2} x {rows * verticalSpacing:F2} units\n" +
               $"Arena size: {arenaWidth} x {arenaHeight} (1.5x = {requiredWidth} x {requiredHeight})";
    }
}
