using VikingAchievements.AchievePanel;
using VikingAchievements.AchieveTab;
using HarmonyLib;

namespace VikingAchievements.GameClasses; 

internal static class GameHud {
    /* A patch to initializing managers */
    [HarmonyPatch(typeof(Hud), "Awake")]
    private static class HudAwake {
        private static void Postfix() {
            PanelManager.Init();
            TabManager.Init();
        }
    }
}