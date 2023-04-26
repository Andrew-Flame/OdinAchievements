using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using VikingAchievements.UI.Icon;

namespace VikingAchievements.GameClasses; 

internal class IconPatches {
    [HarmonyPatch]
    private static class SetIconFocused {
        private static IEnumerable<MethodBase> TargetMethods() {
            yield return typeof(TextsDialog).GetMethod("Setup");
            yield return typeof(SkillsDialog).GetMethod("Setup");
            yield return typeof(InventoryGui).GetMethod("OnOpenTrophies");
        }

        private static void Postfix() => IconManager.Icon.SetFocused();
    }
    
    [HarmonyPatch]
    private static class SetIconDefault {
        private static IEnumerable<MethodBase> TargetMethods() {
            yield return typeof(TextsDialog).GetMethod("OnClose");
            yield return typeof(SkillsDialog).GetMethod("OnClose");
            yield return typeof(InventoryGui).GetMethod("OnCloseTrophies");
        }

        private static void Postfix() => IconManager.Icon.SetDefault();
    }
}