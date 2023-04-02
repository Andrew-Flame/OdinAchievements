using System.Collections.Generic;
using AwesomeAchievements.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace AwesomeAchievements.AchievePanel; 

internal static class PanelHandler {
    public static RectTransform panelRect;
    
    private const float ASPECT_RATIO = 4.12121212f;
    private static AchievePanel _panelComponent;
    private static Vector2 _size;
    private static List<string> _queue = new();

    public static void InitPanel() {
        /* Get necessary game objects */ 
        GameObject hudRoot = GameObject.Find("IngameGui(Clone)");  //Get hud root object
        GameObject chatBox = hudRoot.transform.Find("Chat_box").gameObject;  //Get chat box object
        
        /* Init the achievement panel game object */
        GameObject panel = new GameObject("AchievementPanel", typeof(Image), typeof(AchievePanel));  //Create an achievement panel object
        _panelComponent = panel.GetComponent<AchievePanel>();  //Get the achieve panel component
        panel.transform.SetParent(hudRoot.transform);  //Set the hud root as the parent for the achievement panel
        panel.SetActive(false);  //Hide the panel
        SetPanelTexture(panel);  //Set the achievement panel texture
        
        /* Get chat box corners coordinates */
        var vectors = new Vector3[4];  //Init the array of vectors
        chatBox.GetComponent<RectTransform>().GetWorldCorners(vectors);  //Get corners coordinates

        /* Set the achievement panel size and position */
        panelRect = panel.GetComponent<RectTransform>();  //Get the achievement panel rect transform component
        SetPanelSize(vectors);  //Set achievement panel size
        SetPanelPosition(vectors);  //Set achievement panel position
    }

    private static void SetPanelTexture(GameObject panel) {
        /* Get a resource from assembly */
        const string panelTextureResource = "AwesomeAchievements.Assets.Textures.AchievementPanel.png";  //Get a resource name
        ResourceReader resourceReader = new ResourceReader(panelTextureResource);  //Create a new resource reader
        
        /* Create a texture from the resource */
        Texture2D panelTexture = new Texture2D(0, 0);  //Init random texture (it will be resized)
        panelTexture.LoadImage(resourceReader.ReadAllBytes(), false);  //Load an image from resource
        Sprite panelSprite = 
            Sprite.Create(panelTexture, new Rect(0f, 0f, panelTexture.width, panelTexture.height), new Vector2(0f, 0f));  //Create a new sprite

        /* Set the texture to the achievement panel */
        Image panelImage = panel.GetComponent<Image>();  //Get image component from the achievement panel
        panelImage.sprite = panelSprite;  //Set the new sprite
        panelImage.mainTexture.wrapMode = TextureWrapMode.Clamp;  //Set the wrap mode to clamp
    }

    private static void SetPanelSize(Vector3[] vectors) {
        float width = (vectors[2] - vectors[1]).x,
              height = width / ASPECT_RATIO;  //Get the width and the height of the panel
        _size = new Vector2(width, height);  //Set the new size
        panelRect.sizeDelta = _size;  //Apply the new size to panel
    }

    private static void SetPanelPosition(Vector3[] chatVectors) {
        var minimapVectors = new Vector3[4];  //Init the array of minimap vectors
        Minimap.instance.m_smallRoot.GetComponent<RectTransform>().GetWorldCorners(minimapVectors);  //Get corners coordinates

        float positionX = ((chatVectors[1] + chatVectors[2]) / 2).x - 5f,
              positionY = minimapVectors[0].y - panelRect.sizeDelta.y / 2 - 5f;  //Get the new panel position
        Vector3 position = new Vector3(positionX, positionY);  //Set the new panel position
        Vector3 offsetPosition = position + new Vector3(_size.x + 5f, 0f);  //Set the new panel position with offset
        panelRect.position = offsetPosition;  //Apply new panel position with offset
        
        /* Set values to panel component */
        _panelComponent.distance = Vector3.Distance(position, offsetPosition);  //Set the distance between position and offset position
        _panelComponent.position = position;  //Set the position
        _panelComponent.offsetPosition = offsetPosition;  //Set the offset position
    }
    
    public static void ShowPanel(string achievementName) {
        if (_panelComponent.isBusy) {
            _queue.Add(achievementName);
            return;
        }
        _panelComponent.Appear();
    }

    public static void RunNextPendingAction() {
        if (_panelComponent.isBusy) return;
        if (_queue.Count == 0) return;

        string achievementName = _queue[0];
        _queue.RemoveAt(0);
        
        ShowPanel(achievementName);
    }
}