using AwesomeAchievements.Utility;

namespace AwesomeAchievements.AchieveAnnounce; 

internal static class Announcer {
    public const string DISTINCTIVE = "[/achievement/]";
    public const string PLACEHOLDER = "\rGot an achievement, but you won't see it, since you don't have the AwesomeAchievements mod installed";
    public static void Announce(string achievementName) {
        string playerName = Player.m_localPlayer.GetPlayerName();
        string message = $"{DISTINCTIVE}{Localizer.Player} " +
                         $"<color=orange>{playerName}</color> " +
                         $"{Localizer.ChatMessage} <color=green>[{achievementName.ToUpper()}]</color>{PLACEHOLDER}";
        Chat.instance.SendText(Talker.Type.Shout, message);
    }
}