using System;
using System.Collections.Generic;
using AwesomeAchievements.AchievementLists;
using AwesomeAchievements.Achieves;
using AwesomeAchievements.Utility;
using static Terminal;

namespace AwesomeAchievements.TerminalCommands; 

internal static class AchieveHandler {
    public static void AddAchieves(ConsoleEventArgs args) {
        if (!CheckArgs(args)) return;
        if (Array.IndexOf(args.Args, "All") != -1) {
            AddAllAchieves();
            return;
        }
    }

    private static void AddAllAchieves() {
        foreach (Achievement achievement in AchieveContainer.GetAllAchieves()) 
            achievement.Complete();
    }

    public static void RemoveAchieves(ConsoleEventArgs args) {
        if (!CheckArgs(args)) return;
        if (Array.IndexOf(args.Args, "All") != -1) {
            RemoveAllAchieves();
            return;
        }
    }

    private static void RemoveAllAchieves() {
        
    }

    public static IEnumerable<string> GetAchievesForList() {
        yield return "All";
        foreach (AchievementJsonObject achievement in AchieveContainer.GetAchievementList("template")) 
            yield return achievement.id;
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