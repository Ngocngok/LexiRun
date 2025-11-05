using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
        public string prefabPath;
    }
    
    public CharacterData[] availableCharacters = new CharacterData[]
    {
        new CharacterData { characterName = "Chick", prefabPath = "Assets/Quirky Series Ultimate/Quirky Series Vol.1/Farm Vol.1/Prefabs/Chick.prefab" },
        new CharacterData { characterName = "Donkey", prefabPath = "Assets/Quirky Series Ultimate/Quirky Series Vol.1/Farm Vol.1/Prefabs/Donkey.prefab" },
        new CharacterData { characterName = "Rooster", prefabPath = "Assets/Quirky Series Ultimate/Quirky Series Vol.1/Farm Vol.1/Prefabs/Rooster.prefab" },
        new CharacterData { characterName = "Pigeon", prefabPath = "Assets/Quirky Series Ultimate/Quirky Series Vol.1/Pets Vol.1/Prefabs/Pigeon.prefab" },
        new CharacterData { characterName = "Cat", prefabPath = "Assets/Quirky Series Ultimate/Quirky Series Vol.1/Pets Vol.1/Prefabs/Cat.prefab" },
        new CharacterData { characterName = "Dog", prefabPath = "Assets/Quirky Series Ultimate/Quirky Series Vol.1/Pets Vol.1/Prefabs/Dog.prefab" }
    };
    
    public Button nextButton;
    public Button backButton;
    public Vector3 spawnPosition = Vector3.zero;
    public Vector3 spawnRotation = Vector3.zero;
    public float characterScale = 1f;
    
    private int currentCharacterIndex = 0;
    private GameObject currentCharacterInstance;
    
    void Start()
    {
        // Load saved character selection
        currentCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        
        // Setup buttons
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextCharacter);
        }
        
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnPreviousCharacter);
        }
        
        // Spawn initial character
        SpawnCurrentCharacter();
    }
    
    void OnNextCharacter()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % availableCharacters.Length;
        SaveSelection();
        SpawnCurrentCharacter();
        
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }
    }
    
    void OnPreviousCharacter()
    {
        currentCharacterIndex--;
        if (currentCharacterIndex < 0)
        {
            currentCharacterIndex = availableCharacters.Length - 1;
        }
        SaveSelection();
        SpawnCurrentCharacter();
        
        // Play button click sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }
    }
    
    void SpawnCurrentCharacter()
    {
        // Destroy current character if exists
        if (currentCharacterInstance != null)
        {
            Destroy(currentCharacterInstance);
        }
        
        // Load and spawn new character
        CharacterData characterData = availableCharacters[currentCharacterIndex];
        
#if UNITY_EDITOR
        GameObject characterPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(characterData.prefabPath);
#else
        GameObject characterPrefab = Resources.Load<GameObject>(characterData.characterName);
#endif
        
        if (characterPrefab == null)
        {
            Debug.LogError("Failed to load character prefab: " + characterData.prefabPath);
            return;
        }
        
        currentCharacterInstance = Instantiate(characterPrefab, spawnPosition, Quaternion.Euler(spawnRotation));
        currentCharacterInstance.name = characterData.characterName;
        currentCharacterInstance.transform.localScale = Vector3.one * characterScale;
        
        // Ensure the character plays idle animation
        Animator animator = currentCharacterInstance.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Idle_A", 0, 0f);
        }
    }
    
    void SaveSelection()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentCharacterIndex);
        PlayerPrefs.SetString("SelectedCharacterName", availableCharacters[currentCharacterIndex].characterName);
        PlayerPrefs.SetString("SelectedCharacterPath", availableCharacters[currentCharacterIndex].prefabPath);
        PlayerPrefs.Save();
    }
    
    string GetResourcePath(string fullPath)
    {
        // Convert full path to Resources path
        // This is a fallback - we'll use direct prefab loading instead
        return fullPath;
    }
    
    public static string GetSelectedCharacterPath()
    {
        return PlayerPrefs.GetString("SelectedCharacterPath", "Assets/Quirky Series Ultimate/Quirky Series Vol.1/Farm Vol.1/Prefabs/Chick.prefab");
    }
    
    public static string GetSelectedCharacterName()
    {
        return PlayerPrefs.GetString("SelectedCharacterName", "Chick");
    }
    
    void OnDestroy()
    {
        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(OnNextCharacter);
        }
        
        if (backButton != null)
        {
            backButton.onClick.RemoveListener(OnPreviousCharacter);
        }
    }
}
