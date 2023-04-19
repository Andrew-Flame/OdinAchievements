using VikingAchievements.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace VikingAchievements.AchieveTab; 

/* Class for work with achievement tab */  
internal static class TabManager {
    
    /* Method for initializing this type */
    public static void Init() {
        /* Get required game objects */
        GameObject rootInventory = InventoryGui.instance.gameObject;
        GameObject infoPanel = InventoryGui.instance.m_infoPanel.gameObject;

        /* Init the achievement tab icon */
        GameObject achieveIcon = new GameObject("Achievement_Icon", typeof(Image));
        achieveIcon.transform.SetParent(infoPanel.transform);
        achieveIcon.transform.localPosition = new Vector3(0f, 0f);

        ResourceReader iconReader = new ResourceReader("Assets.Textures.AchievementTabIcon.png");
        Image iconImage = achieveIcon.GetComponent<Image>();
        iconImage.sprite = iconReader.GetSprite();
        iconImage.mainTexture.wrapMode = TextureWrapMode.Clamp;

        LogInfo.Log("An achievement tab manager has been initialized");
    }
}