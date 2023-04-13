// ReSharper disable ObjectCreationAsStatement
using AwesomeAchievements.TerminalCommands;
using HarmonyLib;
using static Terminal;

namespace AwesomeAchievements.GameClasses; 

internal static class GameTerminal {
    /* A patch to add new terminal commands */
    [HarmonyPatch(typeof(Terminal), "Awake")]
    private static class TerminalAwake {
        private static void Postfix() {
            /* Add new console commands */
            new ConsoleCommand("achievement-unlock", "unlock an achievement for the current character",
                               AchieveUnlock.Run,
                               isCheat: true, isSecret: false,
                               optionsFetcher: AchieveUnlock.GetList);
            new ConsoleCommand("achievement-reset", "reset the progress of all achievements for the current player",
                               AchieveReset.Run,
                               isCheat: true, isSecret: false);
        }
    }
}