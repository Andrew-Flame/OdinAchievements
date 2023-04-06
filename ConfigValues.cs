using BepInEx;
using BepInEx.Configuration;

namespace AwesomeAchievements; 

/* A class-container for BepInEx config values */
internal static class ConfigValues {
    private static ConfigEntry<string> _language;
    public static string language => _language.Value;

    /* Method for initializing this type
     * config - the config file where data in containing */
    public static void Init(ConfigFile config) {
        _language = config.Bind("general", "language", "en",
                                "The language in which the mod elements will be displayed");
    }
}