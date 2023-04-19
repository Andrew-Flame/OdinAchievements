using BepInEx.Configuration;

namespace VikingAchievements.Utility; 

/* Class-container for BepInEx config values */
internal static class ConfigValues {
    private static ConfigEntry<string> _language;
    private static ConfigEntry<float> _volume;
    private static ConfigEntry<string> _customSavePath;
    private static ConfigEntry<int> _numberOfBackups;

    public static string Language => _language.Value;
    public static float Volume => _volume.Value;
    public static string CustomSavePath => _customSavePath.Value;
    public static int NumberOfBackups => _numberOfBackups.Value;

    /* Method for initializing this type
     * config - the config file where data in containing */
    public static void Init(ConfigFile config) {
        _language = config.Bind("general", "Language", "eng",
                                "The language in which the mod elements will be displayed\n" +
                                "Available languages: " + string.Join(", ", Localizer.AvailableLangs()));
        
        _volume = config.Bind("general", "Volume", 100f,
                              "The volume of sounds added by this mod in percent");

        _customSavePath = config.Bind("saving", "CustomSavePath", "None",
                                     "The custom path where the mod's saves files will be stored\n" +
                                     "None - use the default save path");

        _numberOfBackups = config.Bind("saving", "NumberOfBackups", 5,
                                       "The maximum number of backups that can be stored in the save folder");

        LogInfo.Log("A config value container has been initialized");
    }
}