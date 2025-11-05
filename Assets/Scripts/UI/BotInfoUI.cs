using UnityEngine;
using UnityEngine.UI;

public class BotInfoUI : MonoBehaviour
{
    public Text botNameText;
    public Text botWordText;
    public Text botMistakesText;
    public Text botCurrentWordProgressText;
    
    private BotController bot;
    
    public void Initialize(BotController bot)
    {
        this.bot = bot;
        
        if (botNameText != null)
        {
            botNameText.text = bot.actorName;
            botNameText.color = bot.actorColor;
        }
    }
    
    public void UpdateInfo()
    {
        if (bot == null) return;
        
        if (bot.isEliminated)
        {
            if (botWordText != null) botWordText.text = "ELIMINATED";
            if (botMistakesText != null) botMistakesText.text = "";
            if (botCurrentWordProgressText != null) botCurrentWordProgressText.text = "";
            return;
        }
        
        // Hide word text since it's now floating above the bot
        if (botWordText != null)
        {
            botWordText.text = "";
        }
        
        if (botMistakesText != null)
        {
            botMistakesText.text = "Mistakes: " + bot.mistakeCount + "/3";
        }
        
        // Display completed words count (e.g., "1/3", "2/3")
        if (botCurrentWordProgressText != null)
        {
            botCurrentWordProgressText.text = bot.completedWords + "/3";
        }
    }
}
