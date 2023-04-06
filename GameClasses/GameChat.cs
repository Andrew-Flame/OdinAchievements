using AwesomeAchievements.AchieveAnnounce;
using HarmonyLib;
using static Chat;

namespace AwesomeAchievements.GameClasses; 

internal static class GameChat {
    [HarmonyPatch(typeof(Chat), "OnNewChatMessage")]
    private class ChatOnNewChatMessage {
        private static bool Prefix(string text, ref float ___m_hideTimer) {
            if (!text.StartsWith(Announcer.DISTINCTIVE)) return true;
            
            ___m_hideTimer = 0f;
            string message = text.Replace(Announcer.DISTINCTIVE, "").Replace(Announcer.PLACEHOLDER, "");
            instance.AddString("<b>" + message + "<b>");
            return false;
        }
    }
}