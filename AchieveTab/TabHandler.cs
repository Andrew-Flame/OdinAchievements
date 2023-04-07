using AwesomeAchievements.Utility;
using UnityEngine;

namespace AwesomeAchievements.AchieveTab; 

/* Class for work with achievement tab */  
internal static class TabHandler {
    
    /* Method for initializing this type */
    public static void Init() {
        /* Get required game objects */
        GameObject rootInventory = InventoryGui.instance.gameObject;
        GameObject infoPanel = InventoryGui.instance.m_infoPanel.gameObject;
        rootInventory.gameObject.transform.localScale = new Vector3(2f, 2f);
        
        LogInfo.Log("An achievement tab handler has been initialized");
    }
}