using AwesomeAchievements.Utility;
using BepInEx;
using HarmonyLib;

namespace AwesomeAchievements;

/* An entry point of this program */
[BepInPlugin(ModInfo.GUID, ModInfo.TITLE, ModInfo.VERSION)]
internal sealed class Master : BaseUnityPlugin {
    public static Harmony harmony;

    /* In this method we initialize all required types */
    private void Awake() {
        harmony = new Harmony(ModInfo.GUID);  //Create a harmony
        harmony.PatchAll();  //Patch the harmony
        
        ConfigValues.Init(Config);
        Localizer.Init(ConfigValues.language);  //Init the localizer
    }
}