using System.Collections.Generic;
using static Terminal;

namespace AwesomeAchievements.TerminalCommands; 

internal static class TerminalHandler {
    public static void AddAchievement(ConsoleEventArgs args) {
        if (!CheckArgs(args)) return;
    }

    public static void RemoveAchievement(ConsoleEventArgs args) {
        if (!CheckArgs(args)) return;
    }

    private static bool CheckArgs(ConsoleEventArgs args) {
        if (args.Length > 1) return true;
        string output = string.Format('\n' + 
@"Not enough arguments!
Use {0} + achievement-id:
    {0} All
    {0} UseVegvisir
    {0} CutTrees", args[0]);
        args.Context.AddString(output);
        return false;
    }
}