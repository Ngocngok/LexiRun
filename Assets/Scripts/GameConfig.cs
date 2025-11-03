using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "LexiRun/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Player Settings")]
    public int playerStartingHP = 3;
    public float playerStartingTime = 60f;
    public float playerMoveSpeed = 5f;
    
    [Header("Bot Settings")]
    public int botCount = 3;
    public float botMoveSpeed = 4f;
    public int botMistakeLimit = 3;
    public float botReplanInterval = 2f;
    
    [Header("Penalty Settings")]
    public float timeDeductionAtZeroProgress = 5f;
    public int hpLossAmount = 1;
    
    [Header("Gameplay Settings")]
    public int wordsToWin = 3;
    public float wordCompletionDelay = 1f;
    public float touchCooldown = 0.5f;
    
    [Header("Arena Settings")]
    public float arenaSize = 30f;
    public float nodeSpacing = 2f;
    
    [Header("Floating Word Display Settings")]
    public float floatingTextHeight = 2f;
    public float floatingTextLetterSpacing = 0.5f;
    public int playerFloatingTextSize = 60;
    public int botFloatingTextSize = 40;
    public Color unfilledLetterColor = Color.white;
    public Color filledLetterColor = Color.blue;
    
    [Header("Word Lists")]
    public string[] wordList = new string[]
    {
        "APPLE", "BREAD", "CHAIR", "DANCE", "EAGLE",
        "FLAME", "GRAPE", "HOUSE", "IMAGE", "JUICE",
        "KNIFE", "LEMON", "MOUSE", "NIGHT", "OCEAN",
        "PIANO", "QUEEN", "RIVER", "STONE", "TIGER",
        "UNCLE", "VOICE", "WATER", "XENON", "YOUTH", "ZEBRA"
    };
}
