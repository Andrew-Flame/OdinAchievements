using AwesomeAchievements.AchievePanel;
using HarmonyLib;

namespace AwesomeAchievements.GameClasses; 

internal static class GameHud {
    [HarmonyPatch(typeof(Hud), "Awake")]
    private static class HudAwake {
        private static void Postfix() => PanelHandler.InitPanel();
    }
}