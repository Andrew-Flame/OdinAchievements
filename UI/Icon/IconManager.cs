using UnityEngine;
using UnityEngine.UI;
using VikingAchievements.Utility;

namespace VikingAchievements.UI.Icon; 

/* Class for work with achievement tab */  
internal static class IconManager {
    private static Image _iconImage;
    
    /* Method for initializing this type */
    public static void Init() {
        /* Get required game objects */
        GameObject infoPanel = InventoryGui.instance.m_infoPanel.gameObject;

        /* Init the achievement tab icon */
        GameObject iconParent = new GameObject("Achievements", typeof(Button));
        iconParent.transform.SetParent(infoPanel.transform);
        iconParent.transform.localPosition = new Vector3(0f, 0f);

        SetDarken(iconParent.transform);
        SetTexture(iconParent.transform);
        SetSize(iconParent.transform);
        SetPosition(iconParent.transform);
        SetButtonComponent(iconParent.transform);

        LogInfo.Log("An achievement tab manager has been initialized");
    }

    private static void SetDarken(Transform iconParent) {
        /* Copy the background object */
        Transform infoPanel = InventoryGui.instance.m_infoPanel;
        GameObject darken = Object.Instantiate(infoPanel.Find("Texts/Background").gameObject, iconParent, true);
        darken.name = "Background";
        darken.transform.localPosition = new Vector3(0f, 0f);
    }
    
    private static void SetTexture(Transform iconParent) {
        /* Create an image object */
        Transform icon = new GameObject("Icon", typeof(Image)).transform;
        icon.SetParent(iconParent);
        icon.localPosition = new Vector3(0f, 0f);

        /* Set the texture */
        ResourceReader iconReader = new ResourceReader("Assets.Textures.Icon.png");
        _iconImage = icon.GetComponent<Image>();
        _iconImage.sprite = iconReader.GetSprite(FilterMode.Point);
        _iconImage.mainTexture.wrapMode = TextureWrapMode.Clamp;
        _iconImage.color = new Color(1f, 0.7176f, 0.3603f);
        
        /* Set the material */
        Material material = InventoryGui.instance.m_infoPanel.Find("Texts/Icon").GetComponent<Image>().material;
        _iconImage.material = material;
    }

    private static void SetSize(Transform iconParent) {
        const float sideSize = 58f;
        Vector2 size = new Vector2(sideSize, sideSize);
        iconParent.Find("Icon").GetComponent<RectTransform>().sizeDelta = size;
    }

    private static void SetPosition(Transform iconParent) {
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
        iconParent.localPosition = new Vector3(-offset, height);
    }

    private static void SetButtonComponent(Transform iconParent) {
        Button button = iconParent.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private static void OnClick() {
        _iconImage.color = new Color(1f, 1f, 1f, 0.2f);
    }
}