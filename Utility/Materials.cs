using UnityEngine;
using UnityEngine.UI;

namespace AwesomeAchievements.Utility; 

/* Class for get the materials from the game because it's the easiest way to get them */
internal static class Materials {
    public static Material Litpanel =>
        GameObject.Find("_GameMain/LoadingGUI/PixelFix/IngameGui(Clone)/Inventory_screen/root/Info/bkg/").GetComponent<Image>().material;
}