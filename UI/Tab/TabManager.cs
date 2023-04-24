using UnityEngine;

namespace VikingAchievements.UI.Tab; 

internal static class TabManager {
    public static void Init() {
        GameObject rootInventory = InventoryGui.instance.m_inventoryRoot.gameObject;
        GameObject trophiesTab = rootInventory.transform.Find("Trophies").gameObject;
        GameObject achievesTab = Object.Instantiate(trophiesTab).gameObject;

        achievesTab.name = "Achievements";
        achievesTab.transform.SetParent(rootInventory.transform);
        achievesTab.transform.position = trophiesTab.transform.position;
    }

    public static void Show() {
        
    }
}