using System;
using System.Linq;
using static Terminal;

namespace AwesomeAchievements.TerminalCommands; 

/* Class for work with achievement terminal commands */
internal static class CommandManager {
    /* Method for checking for args
     * args - arguments for checking
     * returns true if there is at least one argument, otherwise - false */
    public static bool HaveArgs(ConsoleEventArgs args) {
        if (args.Length > 1) return true;
        string output = string.Format('\n' + "Not enough arguments!\n" +
                                      "Use {0} + achievement-id:\n" +
                                      "{0} All\n" +
                                      "{0} Sleep\n" +
                                      "{0} UseVegvisir\n" +
                                      "{0} FindVillage\n" +
                                      "e.t.c", args[0]);
        args.Context.AddString(output);
        return false;
    }

    /* Method for checking for "All" argument in args
     * args - arguments for checking
     * actionIfAll - delegate for executing if there is "All" in arguments
     * returns true if there is an "All" argument in args, otherwise - false */
    public static bool HaveAll(ConsoleEventArgs args, Action actionIfAll) {
        if (!args.Args.Contains("All")) return false;
        actionIfAll();
        return true;
    }
}