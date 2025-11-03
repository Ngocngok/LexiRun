using UnityEngine;

[System.Serializable]
public class DifficultySettings
{
    public string difficultyName;
    public int minWordLength;
    public int maxWordLength;
    public float timeLimit;
    public float botSpeed;
    public float botPauseChance; // 0-1, chance to pause when reaching a letter
    public float botPauseMinDuration;
    public float botPauseMaxDuration;
}

[CreateAssetMenu(fileName = "LevelConfig", menuName = "LexiRun/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Difficulty Tiers")]
    public DifficultySettings easySettings = new DifficultySettings
    {
        difficultyName = "Easy",
        minWordLength = 3,
        maxWordLength = 3,
        timeLimit = 120f,
        botSpeed = 3.5f,
        botPauseChance = 0.7f,
        botPauseMinDuration = 1f,
        botPauseMaxDuration = 2f
    };
    
    public DifficultySettings normalSettings = new DifficultySettings
    {
        difficultyName = "Normal",
        minWordLength = 4,
        maxWordLength = 6,
        timeLimit = 100f,
        botSpeed = 4f,
        botPauseChance = 0.4f,
        botPauseMinDuration = 0.5f,
        botPauseMaxDuration = 1f
    };
    
    public DifficultySettings hardSettings = new DifficultySettings
    {
        difficultyName = "Hard",
        minWordLength = 7,
        maxWordLength = 10,
        timeLimit = 70f,
        botSpeed = 4.5f,
        botPauseChance = 0f,
        botPauseMinDuration = 0f,
        botPauseMaxDuration = 0f
    };
    
    public DifficultySettings GetSettingsForLevel(int level)
    {
        if (level >= 1 && level <= 3)
        {
            return easySettings;
        }
        else if (level >= 4 && level <= 7)
        {
            return normalSettings;
        }
        else
        {
            return hardSettings;
        }
    }
}
