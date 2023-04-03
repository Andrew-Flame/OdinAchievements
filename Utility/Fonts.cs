using UnityEngine;
using UnityEngine.UI;

namespace AwesomeAchievements.Utility; 

internal static class Fonts {
    public static Font Norsebold =>
        GameObject.Find("_GameMain/LoadingGUI/PixelFix/IngameGui(Clone)/Inventory_screen/root/Crafting/topic").GetComponent<Text>().font;
}