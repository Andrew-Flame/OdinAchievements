using AwesomeAchievements.Utility;
using HarmonyLib;
using static Chat;

namespace AwesomeAchievements.GameClasses; 

internal static class GameChat {
    /* A patch to redefine behavior of the chat when it getting a "completing an achievement" message */
    [HarmonyPatch(typeof(Chat), "OnNewChatMessage")]
    private class ChatOnNewChatMessage {
        private static bool Prefix(string text, string user, ref float ___m_hideTimer) {
            /* If it's not a "completing an achievement" message, exec the original method */
            if (!text.StartsWith(Announcer.NOT_INSTALLED_ALERT)) return true;
            
            /* If it's a required message, run the code below */
            if (user == Player.m_localPlayer.GetPlayerName()) ___m_hideTimer = -3f;  //Get an extra showing chat time, if there isn't our achievement
            string message = text.Replace(Announcer.NOT_INSTALLED_ALERT, "");  //Remove the "not mod installed alert" from the string
            instance.AddString("<size=4>\n</size>" + "<b><size=20>" + message + "</size></b>");  //Print the message into the chat
            return false;  //Don't exec the original method
        }
    }
}