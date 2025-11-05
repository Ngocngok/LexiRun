using UnityEngine;
using UnityEditor;

public class SetupHomeChicken : EditorWindow
{
    [MenuItem("Tools/Setup Home Chicken Waypoints")]
    static void Setup()
    {
        // Find the HomeChicken object
        GameObject homeChicken = GameObject.Find("HomeChicken");
        if (homeChicken == null)
        {
            Debug.LogError("HomeChicken not found in scene!");
            return;
        }
        
        ChickenWanderer wanderer = homeChicken.GetComponent<ChickenWanderer>();
        if (wanderer == null)
        {
            Debug.LogError("ChickenWanderer component not found!");
            return;
        }
        
        // Find all waypoints
        GameObject waypointParent = GameObject.Find("WaypointNodes");
        if (waypointParent == null)
        {
            Debug.LogError("WaypointNodes not found in scene!");
            return;
        }
        
        // Get all child transforms
        Transform[] waypoints = new Transform[waypointParent.transform.childCount];
        for (int i = 0; i < waypointParent.transform.childCount; i++)
        {
            waypoints[i] = waypointParent.transform.GetChild(i);
        }
        
        // Assign to wanderer
        SerializedObject so = new SerializedObject(wanderer);
        SerializedProperty waypointsProp = so.FindProperty("waypoints");
        waypointsProp.arraySize = waypoints.Length;
        
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypointsProp.GetArrayElementAtIndex(i).objectReferenceValue = waypoints[i];
        }
        
        so.ApplyModifiedProperties();
        
        Debug.Log($"Successfully assigned {waypoints.Length} waypoints to HomeChicken!");
        EditorUtility.SetDirty(homeChicken);
    }
}
