using System.Collections.Generic;
using AwesomeAchievements.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace AwesomeAchievements.AchievePanel; 

internal static class PanelHandler {
    public static RectTransform panelRect;
    
    private const float ASPECT_RATIO = 4.12121212f;
    private static readonly List<string> Queue = new();
    private static AchievementPanel _panel;
    private static Vector2 _size;

    public static void InitPanel() {
        /* Init the achievement panel game object */
        GameObject panel = new GameObject("Achievement_Panel", typeof(Image), typeof(AchievementPanel));  //Create an achievement panel object
        panel.transform.SetParent(Hud.instance.transform.parent.transform);  //Set the hud root as the parent for the achievement panel
        panel.SetActive(false);  //Hide the panel
        
        /* Get panel components */
        _panel = panel.GetComponent<AchievementPanel>();  //Get the achieve panel component
        panelRect = panel.GetComponent<RectTransform>();  //Get the achievement panel rect transform component

        /* Set achievement panel properties */
        SetPanelTexture();  //Set the achievement panel texture
        SetPanelSize();  //Set achievement panel size
        SetPanelPosition();  //Set achievement panel position
        
        /* Add achievement panel childs */
        AddHeaderText();
    }

    private static void SetPanelTexture() {
        /* Get a resource from assembly */
        const string panelTextureResource = "AwesomeAchievements.Assets.Textures.AchievementPanel.png";  //Get a resource name
        ResourceReader resourceReader = new ResourceReader(panelTextureResource);  //Create a new resource reader
        
        /* Create a texture from the resource */
        Texture2D panelTexture = new Texture2D(0, 0);  //Init random texture (it will be resized)
        panelTexture.LoadImage(resourceReader.ReadAllBytes(), false);  //Load an image from resource
        Sprite panelSprite = 
            Sprite.Create(panelTexture, new Rect(0f, 0f, panelTexture.width, panelTexture.height), new Vector2(0f, 0f));  //Create a new sprite

        /* Set the texture to the achievement panel */
        Image panelImage = _panel.GetComponent<Image>();  //Get image component from the achievement panel
        panelImage.sprite = panelSprite;  //Set the new sprite
        panelImage.mainTexture.wrapMode = TextureWrapMode.Clamp;  //Set the wrap mode to clamp
    }

    private static void SetPanelSize() {
        Vector2 size = Chat.instance.m_chatWindow.GetComponent<RectTransform>().sizeDelta;  //Get size of the chat box
        float width = size.x,
              height = width / ASPECT_RATIO;  //Get the width and the height of the panel
        _size = new Vector2(width, height);  //Set the new size
        panelRect.sizeDelta = _size;  //Apply the new size to panel
    }

    private static void SetPanelPosition() {
        var minimapVectors = new Vector3[4];  //Init the array of minimap vectors
        Minimap.instance.m_smallRoot.GetComponent<RectTransform>().GetWorldCorners(minimapVectors);  //Get corners coordinates

        var chatVectors = new Vector3[4];  //Init the array of chat vectors
        Chat.instance.m_chatWindow.GetComponent<RectTransform>().GetWorldCorners(chatVectors);  //Get corners coordinates

        float positionX = ((chatVectors[1] + chatVectors[2]) / 2).x - 5f,
              positionY = minimapVectors[0].y - panelRect.sizeDelta.y / 2 - 5f;  //Get the new panel position
        Vector3 position = new Vector3(positionX, positionY);  //Set the new panel position
        Vector3 offsetPosition = position + new Vector3(_size.x + 5f, 0f);  //Set the new panel position with offset
        panelRect.position = offsetPosition;  //Apply new panel position with offset
        
        /* Set values to panel component */
        _panel.distance = Vector3.Distance(position, offsetPosition);  //Set the distance between position and offset position
        _panel.position = position;  //Set the position
        _panel.offsetPosition = offsetPosition;  //Set the offset position
    }

    private static void AddHeaderText() {
        const float offsetX = 1.1f,
                    offsetY = 1.5f;
        
        GameObject headerTextObject = new GameObject("Header_Text", typeof(Text), typeof(Outline));  //Create a new game object
        headerTextObject.transform.SetParent(_panel.transform);  //Set the achievement panel as the parent for this object
        headerTextObject.transform.localPosition = new Vector3(0f, 0f, 0f);  //Set local position of the text object
        
        Text headerText = headerTextObject.GetComponent<Text>();  //Get text component of this object
        headerText.rectTransform.sizeDelta = panelRect.sizeDelta / new Vector2(offsetX, offsetY);  //Set text size

        /* Set text properties */
        headerText.text = Localizer.AchievePanelHeader;
        headerText.color = Color.yellow;
        headerText.font = Fonts.Norsebold;
        headerText.fontStyle = FontStyle.Bold;
        headerText.fontSize = 32;
        headerText.alignment = TextAnchor.UpperLeft;
        headerText.horizontalOverflow = HorizontalWrapMode.Overflow;

        /* Setting the text outline */
        Outline headerOutline = headerTextObject.GetComponent<Outline>();  //Get outline component
        headerOutline.effectDistance = new Vector2(1.5f, 1.5f);
        headerOutline.effectColor = Color.black;
        headerOutline.useGraphicAlpha = false;
    }
    
    public static void ShowPanel(string achievementName) {
        if (_panel.isBusy) {
            Queue.Add(achievementName);
            return;
        }
        _panel.Appear();
    }

    public static void RunNextPendingAction() {
        if (_panel.isBusy) return;
        if (Queue.Count == 0) return;

        string achievementName = Queue[0];
        Queue.RemoveAt(0);
        
        ShowPanel(achievementName);
    }
}