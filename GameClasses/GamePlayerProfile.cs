using AwesomeAchievements.Utility;
using HarmonyLib;
using static PlayerProfile;

namespace AwesomeAchievements.GameClasses; 

internal static class GamePlayerProfile {
    [HarmonyPatch(typeof(PlayerProfile), "LoadPlayerData")]
    private static class PlayerProfileLoadPlayerData {
        private static void Postfix(Player player) {
            if (player != Player.m_localPlayer) return;  //If the loading player isn't a local player, exit the method
            AchievesContainer.Init();  //Initialize the achievement container
        }
    }
}