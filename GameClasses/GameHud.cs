using HarmonyLib;
using VikingAchievements.UI.Panel;
using VikingAchievements.UI.Tab;

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