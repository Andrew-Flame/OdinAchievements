using AwesomeAchievements.AchievePanel;
using HarmonyLib;
using UnityEngine;

namespace AwesomeAchievements.GameClasses; 

internal static class GameScreen {
    [HarmonyPatch(typeof(Screen), "SetResolution", typeof(int), typeof(int), typeof(FullScreenMode), typeof(int))]
    private static class ScreenSetResolution {
        private static void Postfix() {
            if (Hud.instance != null) PanelManager.Init();
        }
    }
}