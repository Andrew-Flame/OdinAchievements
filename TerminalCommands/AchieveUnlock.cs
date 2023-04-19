using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VikingAchievements.Achieves;
using VikingAchievements.Utility;
using static Terminal;

namespace VikingAchievements.TerminalCommands; 

/* Class for work with achievement-unlock terminal commands */
internal static class AchieveUnlock {
    /* Method for run the terminal command */
    public static void Run(ConsoleEventArgs args) {
        if (!HaveArgs(args)) return;  //If there are no args, exit the method
        if (HaveAll(args)) return;  //If there is "All" argument special delegate will be executed, so exit this method

        string id = args[1];  //Get the achievement id
        if (AchievesContainer.Has(id, out Achievement achievement))
            achievement.Complete();
    }
    
    /* Method for checking for args
     * args - arguments for checking
     * returns true if there is at least one argument, otherwise - false */
    private static bool HaveArgs(ConsoleEventArgs args) {
        if (args.Length > 1) return true;
        string output = string.Format('\n' + "Not enough arguments!\n" +
                                      "Use {0} + achievement-id:\n" +
                                      "\t{0} All\n" +
                                      "\t{0} Sleep\n" +
                                      "\t{0} UseVegvisir\n" +
                                      "\t{0} FindVillage\n" +
                                      "\te.t.c", args[0]);
        args.Context.AddString(output);
        return false;
    }
    
    /* Method for checking for "All" argument in args
     * args - arguments for checking
     * actionIfAll - delegate for executing if there is "All" in arguments
     * returns true if there is an "All" argument in args, otherwise - false */
    private static bool HaveAll(ConsoleEventArgs args) {
        if (!args.Args.Contains("All")) return false;
        CompleteAll();
        return true;
    }

    /* Method for completing all uncompleted achievements from the container */
    private static void CompleteAll() {
        foreach (Achievement achievement in GetContainerData()) //Get all uncompleted achievements
            achievement.Complete();  //Complete them
    }
    
    /* Method for getting an array with uncompleted achievements from the container
     * returns the array with uncompleted achievements' instances */
    private static Achievement[] GetContainerData() {
        Type container = typeof(AchievesContainer);
        return container.GetField("_data", BindingFlags.Static | BindingFlags.NonPublic)!
                        .GetValue(null) as Achievement[];
    }

    /* Method for getting list of the achievements' ids for terminal hints
     * returns the list of achievements' ids */
    public static List<string> GetList() {
        var result = GetContainerData().Select(e => e.Id).ToList();
        result.Insert(0, "All");
        return result;
    }
}