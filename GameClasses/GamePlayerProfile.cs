using AwesomeAchievements.Utility;
using HarmonyLib;
using static PlayerProfile;

namespace AwesomeAchievements.GameClasses; 

internal static class GamePlayerProfile {
    [HarmonyPatch(typeof(PlayerProfile), "LoadPlayerData")]
    private static class PlayerProfileLoadPlayerData {
        private static void Postfix(PlayerProfile __instance) {
            AchievesContainer.Init(ConfigValues.language);  //Initialize the achievement container
        }
    }
}