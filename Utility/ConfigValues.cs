using BepInEx.Configuration;

namespace AwesomeAchievements.Utility; 

/* Class-container for BepInEx config values */
internal static class ConfigValues {
    private static ConfigEntry<string> _language;
    public static string Language => _language.Value;

    /* Method for initializing this type
     * config - the config file where data in containing */
    public static void Init(ConfigFile config) {
        _language = config.Bind("general", "language", "en",
                                "The language in which the mod elements will be displayed");
        
        LogInfo.Log("A config value container has been initialized");
    }
}