using BepInEx.Logging;

namespace VikingAchievements.Utility; 

/* Class for work with info logging */
internal static class LogInfo {
    private static ManualLogSource _logger;

    /* Method for initializing this type
     * logger - default modification logger */
    public static void Init(ManualLogSource logger) {
        _logger = logger;
        Log("An info logger has been initialized");
    }

    public static void Log(object info) => _logger.LogInfo(info);
}