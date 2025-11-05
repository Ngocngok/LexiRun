using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetupCharacterSelection : MonoBehaviour
{
    public static string Execute()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        
        // Find CharacterSelectionManager
        CharacterSelectionManager csm = GameObject.FindFirstObjectByType<CharacterSelectionManager>();
        if (csm == null)
        {
            return "CharacterSelectionManager not found in scene!";
        }
        
        // Find buttons
        Button nextButton = GameObject.Find("NextCharacterButton")?.GetComponent<Button>();
        Button backButton = GameObject.Find("PreviousCharacterButton")?.GetComponent<Button>();
        
        if (nextButton == null || backButton == null)
        {
            return "Buttons not found! Next: " + (nextButton != null) + ", Back: " + (backButton != null);
        }
        
        // Assign buttons
        csm.nextButton = nextButton;
        csm.backButton = backButton;
        
        // Mark as dirty
        EditorUtility.SetDirty(csm);
        
        return "Successfully assigned buttons to CharacterSelectionManager!";
    }
}
