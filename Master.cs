using AwesomeAchievements.Saving;
using BepInEx;
using HarmonyLib;

namespace AwesomeAchievements;

[BepInPlugin(ModInfo.GUID, ModInfo.TITLE, ModInfo.VERSION)]
internal sealed class Master : BaseUnityPlugin {
    public static Harmony harmony;
    public static AchievementsContainer achievementsContainer;
    
    private void Awake() {
        harmony = new Harmony(ModInfo.GUID);  //Create a harmony
        achievementsContainer = new AchievementsContainer("ru");  //Create an achievement container
    }
}