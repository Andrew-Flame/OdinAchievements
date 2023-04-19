using UnityEngine;
using UnityEngine.UI;

namespace VikingAchievements.Utility; 

/* Class for get the fonts from the game because it's the easiest way to get them */
internal static class Fonts {
    public static Font Norsebold =>
        GameObject.Find("_GameMain/LoadingGUI/PixelFix/IngameGui(Clone)/Inventory_screen/root/Crafting/topic").GetComponent<Text>().font;

    public static Font AveriaSerifLibre =>
        GameObject.Find("_GameMain/LoadingGUI/PixelFix/IngameGui(Clone)/Inventory_screen/root/Crafting/TabsButtons/Craft/Text")
                  .GetComponent<Text>().font;
}