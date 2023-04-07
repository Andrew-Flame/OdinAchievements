namespace AwesomeAchievements.Utility; 

/* Class for the work with chat announcements */
internal static class Announcer {
    /* This alert will be shown to those who haven't installed the mod. It ends with a null character, therefore nothing after it will be displayed.
     * For those who have installed the modification, this part of the line is removed and a normal message is displayed.*/
    public const string NOT_INSTALLED_ALERT = "This player has just completed an achievement in the 'AwesomeAchievements' mod. " + 
                                              "If you want to know more, find this mod on NexusMods or Thunderstore\0";
    
    /* Method for announce the achievement name in the game chat
     * achievementName - the name of the achievement for printing in the chat */
    public static void Announce(string achievementName) {
        string playerName = Player.m_localPlayer.GetPlayerName();  //Get the player name
        string message = string.Format(Localizer.ChatMessage, $"<color=orange>{playerName}</color>") + ":\n" +
                         $"<color=green>[{achievementName}]</color>";  //Create a message
        Chat.instance.SendText(Talker.Type.Normal, NOT_INSTALLED_ALERT + message);  //'Say' message with the alert in the chat
    }
}