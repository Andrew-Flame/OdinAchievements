// ReSharper disable ObjectCreationAsStatement
using System.Linq;
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
            new ConsoleCommand("achieve-complete", "complete an achievement for the current character",
                               CompleteAchieve.Run,
                               isCheat: true, isSecret: false,
                               optionsFetcher: CompleteAchieve.GetList().ToList);
        }
    }
}