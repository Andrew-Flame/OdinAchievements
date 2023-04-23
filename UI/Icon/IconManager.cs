using UnityEngine;
using UnityEngine.UI;
using VikingAchievements.Utility;

namespace VikingAchievements.UI.Icon; 

/* Class for work with achievement tab */  
internal static class IconManager {
    /* Method for initializing this type */
    public static void Init() {
        /* Get required game objects */
        GameObject infoPanel = InventoryGui.instance.m_infoPanel.gameObject;

        /* Init the achievement tab icon */
        GameObject achieveIcon = new GameObject("Achievements", typeof(Button));
        achieveIcon.transform.SetParent(infoPanel.transform);
        achieveIcon.transform.localPosition = new Vector3(0f, 0f);

        SetDarken(achieveIcon.transform);
        SetTexture(achieveIcon.transform);
        SetSize(achieveIcon.transform);
        SetPosition(achieveIcon.transform);
        SetButtonComponent(achieveIcon.transform);

        LogInfo.Log("An achievement tab manager has been initialized");
    }

    private static void SetDarken(Transform achieveIcon) {
        /* Copy the background object */
        Transform infoPanel = InventoryGui.instance.m_infoPanel;
        GameObject darken = Object.Instantiate(infoPanel.Find("Texts/Background").gameObject, achieveIcon, true);
        darken.name = "Background";
        darken.transform.localPosition = new Vector3(0f, 0f);
    }
    
    private static void SetTexture(Transform achieveIcon) {
        /* Create an image object */
        GameObject icon = new GameObject("Icon", typeof(Image));
        icon.transform.SetParent(achieveIcon);
        icon.transform.localPosition = new Vector3(0f, 0f);
        
        /* Set the texture */
        ResourceReader iconReader = new ResourceReader("Assets.Textures.IconDefault.png");
        Image image = icon.GetComponent<Image>();
        image.sprite = iconReader.GetSprite(FilterMode.Point);
        image.mainTexture.wrapMode = TextureWrapMode.Clamp;
    }

    private static void SetSize(Transform achieveIcon) {
        Vector2 size = new Vector2(60f, 60f);
        achieveIcon.Find("Icon").GetComponent<RectTransform>().sizeDelta = size;
    }

    private static void SetPosition(Transform achieveIcon) {
        /* Get game objects */
        Transform infoPanel = InventoryGui.instance.m_infoPanel;
        Transform texts = infoPanel.Find("Texts");
        Transform skills = infoPanel.Find("Skills");
        Transform trophies = infoPanel.Find("Trophies");
        Transform pvp = infoPanel.Find("PVP");

        /* Get required info */
        float infoWidth = infoPanel.GetComponent<RectTransform>().sizeDelta.x;  //Get the width of the info panel
        float offset = infoWidth / 6f;  //Get the elements offset
        float height = texts.localPosition.y;  //Get the local height of elements

        /* Set positions */
        texts.localPosition = new Vector3(-offset * 5f, height);
        skills.localPosition = new Vector3(-offset * 4f, height);
        trophies.localPosition = new Vector3(-offset * 3f, height);
        pvp.localPosition = new Vector3(-offset * 2f, height);
        achieveIcon.localPosition = new Vector3(-offset, height);
    }

    private static void SetButtonComponent(Transform achieveIcon) {
        Button button = achieveIcon.GetComponent<Button>();
        Image image = achieveIcon.GetComponent<Image>();
        
        button.image = image;
        button.targetGraphic = image;
    }
}