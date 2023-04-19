using VikingAchievements.Utility;
using BepInEx;
using HarmonyLib;

namespace VikingAchievements;

/* An entry point of this program */
[BepInPlugin(ModInfo.GUID, ModInfo.TITLE, ModInfo.VERSION)]
internal sealed class Master : BaseUnityPlugin {
    public static Harmony harmony;

    /* In this method we initialize all required types */
    private void Awake() {
        harmony = new Harmony(ModInfo.GUID);  //Create a harmony
        harmony.PatchAll();  //Patch the harmony
        
        /* Init containers */
        LogInfo.Init(Logger);  //Init an info logger
        ConfigValues.Init(Config);  //Init config values
        Localizer.Init();  //Init the localizer
    }
}