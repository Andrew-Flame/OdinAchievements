using BepInEx.Logging;

namespace AwesomeAchievements.Utility; 

/* Class for work with info logging */
internal static class LogInfo {
    private static ManualLogSource _logger;

    public static void Init(ManualLogSource logger) {
        _logger = logger;
        Log("An info logger has been initialized");
    }

    public static void Log(object info) => _logger.LogInfo(info);
}