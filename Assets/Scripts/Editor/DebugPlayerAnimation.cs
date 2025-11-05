using UnityEngine;
using UnityEditor;

public class DebugPlayerAnimation : MonoBehaviour
{
    public static string Execute()
    {
        // Load Player prefab
        GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        if (playerPrefab == null)
        {
            return "Player prefab not found!";
        }
        
        // Check for CharacterAnimationController
        CharacterAnimationController animController = playerPrefab.GetComponent<CharacterAnimationController>();
        if (animController == null)
        {
            return "CharacterAnimationController not found on Player prefab!";
        }
        
        // Check for CharacterModel
        Transform characterModel = playerPrefab.transform.Find("CharacterModel");
        if (characterModel == null)
        {
            return "CharacterModel not found in Player prefab!";
        }
        
        // Check for Animator
        Animator animator = characterModel.GetComponent<Animator>();
        if (animator == null)
        {
            return "Animator not found on CharacterModel!";
        }
        
        // Check animator controller
        if (animator.runtimeAnimatorController == null)
        {
            return "Animator has no AnimatorController assigned!";
        }
        
        string result = "Player Animation Debug:\n";
        result += "- CharacterAnimationController: Found\n";
        result += "- CharacterModel: Found\n";
        result += "- Animator: Found\n";
        result += "- AnimatorController: " + animator.runtimeAnimatorController.name + "\n";
        
        // Check for animation clips
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        result += "- Animation Clips (" + clips.Length + "):\n";
        foreach (AnimationClip clip in clips)
        {
            result += "  - " + clip.name + "\n";
        }
        
        return result;
    }
}
