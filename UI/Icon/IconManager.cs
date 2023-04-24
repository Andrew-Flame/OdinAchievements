using UnityEngine;
using UnityEngine.UI;
using VikingAchievements.UI.Tab;
using VikingAchievements.Utility;

namespace VikingAchievements.UI.Icon; 

/* Class for work with achievement tab */  
internal static class IconManager {
    public static AchieveIcon icon;

    /* Method for initializing this type */
    public static void Init() {
        /* Get required game objects */
        GameObject infoPanel = InventoryGui.instance.m_infoPanel.gameObject;

        /* Init the achievement tab icon */
        GameObject iconParent = new GameObject("Achievements", typeof(AchieveIcon), typeof(Button));
        iconParent.transform.SetParent(infoPanel.transform);
        iconParent.transform.localPosition = new Vector3(0f, 0f);
        icon = iconParent.GetComponent<AchieveIcon>();

        /* Set parameters */
        SetDarken();
        SetTexture();
        SetSize();
        SetPosition();
        SetButton();

        LogInfo.Log("An achievement tab manager has been initialized");
    }

    /* Method for setting the background shadow */
    private static void SetDarken() {
        /* Copy the background object */
        Transform infoPanel = InventoryGui.instance.m_infoPanel;
        GameObject darken = Object.Instantiate(infoPanel.Find("Texts/Background").gameObject, icon.transform, true);
        darken.name = "Background";
        darken.transform.localPosition = new Vector3(0f, 0f);
    }
    
    /* Method for setting the icon texture */
    private static void SetTexture() {
        /* Create an image object */
        Transform iconImage = new GameObject("Icon", typeof(Image)).transform;
        iconImage.SetParent(icon.transform);
        iconImage.localPosition = new Vector3(0f, 0f);

        /* Set the texture */
        ResourceReader iconReader = new ResourceReader("Assets.Textures.Icon.png");
        icon.image = iconImage.GetComponent<Image>();
        icon.image.sprite = iconReader.GetSprite(FilterMode.Point);
        icon.image.mainTexture.wrapMode = TextureWrapMode.Clamp;
        icon.SetDefault();
        
        /* Set the material */
        Material material = InventoryGui.instance.m_infoPanel.Find("Texts/Icon").GetComponent<Image>().material;
        icon.image.material = material;
    }

    /* Method for setting the size of the icon */
    private static void SetSize() {
        const float sideSize = 58f;
        Vector2 size = new Vector2(sideSize, sideSize);
        icon.transform.Find("Icon").GetComponent<RectTransform>().sizeDelta = size;
    }

    /* Method for setting the position of the icon */
    private static void SetPosition() {
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
        icon.transform.localPosition = new Vector3(-offset, height);
    }

    /* Method for setting the button component */
    private static void SetButton() {
        Button button = icon.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    /* Method for OnClick icon event */
    private static void OnClick() {
        icon.SetFocused();
        TabManager.Show();
    }
}