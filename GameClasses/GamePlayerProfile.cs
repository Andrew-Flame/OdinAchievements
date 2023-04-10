using System.IO;
using AwesomeAchievements.Utility;
using HarmonyLib;

namespace AwesomeAchievements.GameClasses; 

internal static class GamePlayerProfile {
    private static string _playerFolder;
    
    [HarmonyPatch(typeof(PlayerProfile), "LoadPlayerData")]
    private static class PlayerProfileLoadPlayerData {
        private static void Postfix(Player player, PlayerProfile __instance) {
            if (player != Player.m_localPlayer) return;  //If the loading player isn't a local player, exit the method
            _playerFolder = __instance.GetPath();  //Save the relative path to the save folder
            AchievesContainer.Init();  //Initialize the achievement container
        }
    }

    /* Method for getting a directory info of the folder where characters are saved
     return a directory info of the save folder */
    public static DirectoryInfo GetSaveDirectory() {
        string path = $"{Utils.persistantDataPath}/{_playerFolder}";  //Get the folder where the current player is saved
        return new DirectoryInfo(path).Parent;  //Get the parent directory of this path
    }
}