using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterAvatar : MonoBehaviour
{
    [Tooltip("List of avatar sprites, index corresponds to character selection")]
    public List<Sprite> avatarSprites;

    private int currentCharacterIndex = 0;

    // Update all Image components under this GameObject to the selected avatar
    private void UpdateAvatarImages()
    {
        currentCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        Sprite selectedSprite = avatarSprites[currentCharacterIndex];
        Image[] images = GetComponentsInChildren<Image>(true);
        foreach (var img in images)
        {
            img.sprite = selectedSprite;
        }
    }

    // Optionally, update on Start
    void Start()
    {
        UpdateAvatarImages();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
