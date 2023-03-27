using BepInEx;
using HarmonyLib;

namespace AwesomeAchievements;

[BepInPlugin(ModInfo.GUID, ModInfo.TITLE, ModInfo.VERSION)]
internal sealed class Master : BaseUnityPlugin {
    public static Harmony harmony;
    
    private void Awake() {
        harmony = new Harmony(ModInfo.GUID);  //Create a harmony
    }
}