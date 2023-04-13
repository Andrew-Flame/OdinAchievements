using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AwesomeAchievements.Achieves;
using AwesomeAchievements.Utility;
using static Terminal;
using static AwesomeAchievements.TerminalCommands.CommandManager;

namespace AwesomeAchievements.TerminalCommands; 

/* Class for work with achievement-unlock terminal commands */
internal static class AchieveUnlock {
    /* Method for run the terminal command */
    public static void Run(ConsoleEventArgs args) {
        if (!HaveArgs(args)) return;  //If there are no args, exit the method
        if (HaveAll(args, CompleteAll)) return;  //If there is "All" argument special delegate will be executed, so exit this method

        string id = args[1];  //Get the achievement id
        if (AchievesContainer.Has(id, out Achievement achievement))
            achievement.Complete();
    }

    /* Method for completing all uncompleted achievements from the container */
    private static void CompleteAll() {
        foreach (Achievement achievement in GetContainerData()) //Get all uncompleted achievements
            achievement.Complete();  //Complete them
    }

    /* Method for getting list of the achievements' ids for terminal hints
     * returns the list of achievements' ids */
    public static List<string> GetList() {
        var result = GetContainerData().Select(e => e.Id).ToList();
        result.Insert(0, "All");
        return result;
    }
    
    /* Method for getting an array with uncompleted achievements from the container
     * returns the array with uncompleted achievements' instances */
    private static Achievement[] GetContainerData() {
        Type container = typeof(AchievesContainer);
        return container.GetField("_data", BindingFlags.Static | BindingFlags.NonPublic)!
                        .GetValue(null) as Achievement[];
    }
}