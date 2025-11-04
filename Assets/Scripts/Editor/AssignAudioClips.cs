using UnityEngine;
using UnityEditor;

public class AssignAudioClips : MonoBehaviour
{
    [MenuItem("LexiRun/Assign Audio Clips")]
    public static void AssignClips()
    {
        // Find AudioManager in the scene
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in scene!");
            return;
        }
        
        // Load audio clips
        AudioClip bgHome = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/bg_home.mp3");
        AudioClip bgGame = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/bg_game.mp3");
        AudioClip buttonClick = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/button_click.mp3");
        AudioClip collectRight = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/collect_right_letter.mp3");
        AudioClip collectWrong = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/collect_wrong_letter.mp3");
        AudioClip completeWord = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/complete_word.mp3");
        AudioClip winGame = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/win_game.mp3");
        AudioClip loseGame = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/lose_game.mp3");
        
        // Assign to AudioManager using SerializedObject
        SerializedObject so = new SerializedObject(audioManager);
        
        so.FindProperty("menuMusic").objectReferenceValue = bgHome;
        so.FindProperty("gameplayMusic").objectReferenceValue = bgGame;
        so.FindProperty("buttonClickSFX").objectReferenceValue = buttonClick;
        so.FindProperty("correctLetterSFX").objectReferenceValue = collectRight;
        so.FindProperty("wrongLetterSFX").objectReferenceValue = collectWrong;
        so.FindProperty("wordCompleteSFX").objectReferenceValue = completeWord;
        so.FindProperty("gameWinSFX").objectReferenceValue = winGame;
        so.FindProperty("gameLoseSFX").objectReferenceValue = loseGame;
        
        so.ApplyModifiedProperties();
        
        EditorUtility.SetDirty(audioManager);
        
        Debug.Log("Audio clips assigned successfully!");
    }
}
