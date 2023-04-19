using System.IO;
using VikingAchievements.Saving;
using VikingAchievements.Utility;
using HarmonyLib;

namespace VikingAchievements.GameClasses; 

internal static class GamePlayerProfile {
    private static string _playerFolder;
    private static string _playerName;

    [HarmonyPatch(typeof(PlayerProfile), "LoadPlayerData")]
    private static class PlayerProfileLoadPlayerData {
        private static void Postfix(Player player, PlayerProfile __instance) {
            if (player != Player.m_localPlayer) return;  //If the loading player isn't a local player, exit the method
            _playerFolder = __instance.GetPath();  //Get the relative path to the save folder
            _playerName = player.GetPlayerName();  //Get the player name
            AchievesContainer.Init();  //Initialize the achievement container
        }
    }

    [HarmonyPatch(typeof(PlayerProfile), "SavePlayerData")]
    private static class PlayerProfileSavePlayerData {
        private static void Postfix() => SaveWriter.Save();
    }

    /* Method for getting a directory info of the folder where characters are saved
     return a directory info of the save folder */
    public static DirectoryInfo GetSaveDirectory() {
        string path = $"{Utils.persistantDataPath}/{_playerFolder}";  //Get the folder where the current player is saved
        DirectoryInfo parent = new DirectoryInfo(path).Parent!.Parent;  //Get the parent directory of this path
        return new DirectoryInfo(parent!.FullName + "/characters_local");  //Get the required directory
    }

    /* Method for getting the player name */
    public static string GetPlayerName() => _playerName;
}