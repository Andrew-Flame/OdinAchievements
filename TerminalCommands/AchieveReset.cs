using VikingAchievements.Utility;
using static Terminal;

namespace VikingAchievements.TerminalCommands; 

/* Class for work with achievement-remove terminal commands */
internal static class AchieveReset {
    public static void Run(ConsoleEventArgs args) {
        if (!IsConfirmed(args)) return;  //Checking for confirmation
        AchievesContainer.Init();  //Reinitializing the achievement container
    }

    /* Method for checking the achievement restore command for confirmation
     * args - arguments for checking
     * returns true - if the command is confirmed, false - otherwise */
    private static bool IsConfirmed(ConsoleEventArgs args) {
        string confirmation = args.Length > 1 ? args[1] : string.Empty;  //Get the second argument which must contains a confirmation
        string playerName = Player.m_localPlayer.GetPlayerName();  //Get the player name
        if (confirmation.ToLower().Equals(playerName.ToLower())) return true;  //If the confirmation equals the player name, this command confirmed

        /* If this command isn't confirmed */
        string output = "\nTo reset the progress of achievements you need to confirm this action\n" +
                        "To confirm, enter your player's name in the terminal after this command:\n" +
                        $"\t{args[0]} {playerName}";
        args.Context.AddString(output);  //Output the text
        return false;
    }
}