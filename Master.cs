using BepInEx;
using HarmonyLib;

namespace AwesomeAchievements {
    [BepInPlugin(ModInfo.GUID, ModInfo.TITLE, ModInfo.VERSION)]
    public class Master : BaseUnityPlugin {
        void Awake() {
            #region Harmony patch
            Harmony harmony = new Harmony(ModInfo.GUID);
            harmony.PatchAll();
            #endregion
        }
    }
}