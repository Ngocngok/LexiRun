using UnityEngine;
using UnityEditor;

public class InvokeBotInfoUISetup : MonoBehaviour
{
    public static string Execute()
    {
        FinalBotInfoUISetup.SetupPrefab();
        return "BotInfoUI prefab setup invoked successfully!";
    }
}
