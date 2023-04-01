using AwesomeAchievements.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace AwesomeAchievements; 

internal static class AchievePanel {
    private const float PANEL_POSITION_X = 1665,
                        PANEL_POSITION_Y = 535;
    private const float SCALE_WIDTH = 5, 
                        SCALE_HEIGHT = SCALE_WIDTH / ASPECT_RATIO;
    private const float ASPECT_RATIO = 4.12121212f;
    
    private static GameObject _panel;

    public static void InitPanel() {
        GameObject hudRoot = Hud.instance.m_rootObject;  //Get hud root object
        _panel = new GameObject("AchievementPanel", typeof(Image));  //Create an achievement panel object
        _panel.transform.SetParent(hudRoot.transform);  //Set the hud root as the parent for the achievement panel
        _panel.SetActive(false);  //Make panel non active
        
        const string panelTextureResource = "AwesomeAchievements.Assets.Textures.AchievementPanel.png";  //Get a resource name
        ResourceReader resourceReader = new ResourceReader(panelTextureResource);  //Create a new resource reader
        
        Texture2D panelTexture = new Texture2D(0, 0);
        panelTexture.LoadImage(resourceReader.ReadAllBytes(), false);
        Sprite panelSprite = Sprite.Create(panelTexture, new Rect(0f, 0f, panelTexture.width, panelTexture.height), new Vector2(0.5f, 0.5f));

        _panel.GetComponent<Image>().sprite = panelSprite;
        _panel.transform.localScale = new Vector3(SCALE_WIDTH, SCALE_HEIGHT);
        _panel.transform.position = new Vector3(PANEL_POSITION_X, PANEL_POSITION_Y);
    }
    
    public static void ShowPanel(string achievementName) {
        Debug.Log(achievementName);
    }
}