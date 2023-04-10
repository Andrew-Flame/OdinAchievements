using BepInEx.Configuration;

namespace AwesomeAchievements.Utility; 

/* Class-container for BepInEx config values */
internal static class ConfigValues {
    private static ConfigEntry<string> _language;
    private static ConfigEntry<string> _savePath;
    private static ConfigEntry<float> _volume;
    private static ConfigEntry<int> _numberOfBackups;

    public static string Language => _language.Value;
    public static string SavePath => _savePath.Value;
    public static float Volume => _volume.Value;
    public static int NumberOfBackups => _numberOfBackups.Value;

    /* Method for initializing this type
     * config - the config file where data in containing */
    public static void Init(ConfigFile config) {
        _language = config.Bind("general", "Language", "eng",
                                "The language in which the mod elements will be displayed\n" +
                                "Available languages: " + string.Join(", ", Localizer.AvailableLangs()));

        _savePath = config.Bind("general", "SaveDirectory", "None",
                                     "The custom path where the mod's saves files will be stored\n" +
                                     "None - use the default save path");
            
        _volume = config.Bind("general", "Volume", 100f,
                              "The volume of sounds added by this mod in percent");

        _numberOfBackups = config.Bind("general", "NumberOfBackups", 5,
                                       "The maximum number of backups that can be stored in the save folder");
        
        LogInfo.Log("A config value container has been initialized");
    }
}