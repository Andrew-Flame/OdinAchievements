using System;
using System.Collections.Generic;
using System.Reflection;
using AwesomeAchievements.Achieves;
using AwesomeAchievements.Utility;
using static Terminal;
using static AwesomeAchievements.TerminalCommands.AchievesHandler;

namespace AwesomeAchievements.TerminalCommands; 

/* A class for working with achievement-add terminal commands */
internal static class CompleteAchieve {
    /* Method for run the terminal command */
    public static void Run(ConsoleEventArgs args) {
        if (!HaveArgs(args)) return;  //If there are no args, exit the method
        if (HaveAll(args, CompleteAll)) return;  //If there is "All" argument special delegate will be executed, so exit this method

        /* If all is OK */
        try {
            for (ushort i = 1; i < args.Length; i++) {  //Cycle through arguments
                Achievement achievement = AchievesContainer.GetAchievement(args[i]);  //Get achievement by id
                achievement.Complete();  //Complete this achievement
            }
        } catch { }
    }

    /* Method for completing all uncompleted achievements from the container */
    private static void CompleteAll() {
        foreach (Achievement achievement in GetForCompleting()) //Get all uncompleted achievements
            achievement.Complete();  //Complete them
    }

    /* Method for getting enumerable of the achievements' ids for terminal hints
     * returns the enumerable of achievements' ids */
    public static IEnumerable<string> GetList() {
        yield return "All";
        foreach (Achievement achievement in GetForCompleting()) yield return achievement.Id;
    }

    /* Method for getting an array with uncompleted achievements from the container
     * returns the array with uncompleted achievements' instances */
    private static Achievement[] GetForCompleting() {
        Type container = typeof(AchievesContainer);
        return container.GetField("_data", BindingFlags.Static | BindingFlags.NonPublic)!
                        .GetValue(null) as Achievement[];
    }
}