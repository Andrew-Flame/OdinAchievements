using AwesomeAchievements.AchieveAnnounce;
using HarmonyLib;
using static Chat;

namespace AwesomeAchievements.GameClasses; 

internal static class GameChat {
    /* A patch to redefine behavior of the chat when it getting a "completing an achievement" message */
    [HarmonyPatch(typeof(Chat), "OnNewChatMessage")]
    private class ChatOnNewChatMessage {
        private static bool Prefix(string text, ref float ___m_hideTimer) {
            /* If it's not a "completing an achievement" message, exec the original method */
            if (!text.StartsWith(Announcer.NOT_INSTALLED_ALERT)) return true;
            
            /* If it's a required message, run the code below */
            ___m_hideTimer = -2f;  //Get an extra showing chat time
            string message = text.Replace(Announcer.NOT_INSTALLED_ALERT, "");  //Remove the "not mod installed alert" from the string
            instance.AddString('\n' + "<b><size=22>" + message + "</size></b>");  //Print the message into the chat
            return false;  //Don't exec the original method
        }
    }
}