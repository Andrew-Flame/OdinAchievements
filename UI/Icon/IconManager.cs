using UnityEngine;
using UnityEngine.UI;
using VikingAchievements.Utility;

namespace VikingAchievements.UI.Icon; 

/* Class for work with achievement tab */  
internal static class TabManager {
    
    /* Method for initializing this type */
    public static void Init() {
        /* Get required game objects */
        GameObject infoPanel = InventoryGui.instance.m_infoPanel.gameObject;

        /* Init the achievement tab icon */
        GameObject achieveIcon = new GameObject("Achievement_Icon", typeof(Image));
        achieveIcon.transform.SetParent(infoPanel.transform);
        achieveIcon.transform.localPosition = new Vector3(0f, 0f);

        SetTexture(achieveIcon);
        

        LogInfo.Log("An achievement tab manager has been initialized");
    }

    private static void SetTexture(GameObject achieveIcon) {
        ResourceReader iconReader = new ResourceReader("Assets.Textures.AchievementTabIcon.png");
        Image iconImage = achieveIcon.GetComponent<Image>();
        iconImage.sprite = iconReader.GetSprite();
        iconImage.mainTexture.wrapMode = TextureWrapMode.Clamp;
    }
}