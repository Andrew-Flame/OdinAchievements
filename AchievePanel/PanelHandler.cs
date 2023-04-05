using System.Collections.Generic;
using System.Threading;
using AwesomeAchievements.Utility;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace AwesomeAchievements.AchievePanel; 

/* A class for working with the achievement panel */
internal static class PanelHandler {
    public static RectTransform panelRect;
    public static AudioSource audioSource;

    private const float ASPECT_RATIO = 4.12121212f;
    private static readonly List<string> Queue = new();
    private static AchievementPanel _panel;
    private static Vector2 _size;
    private static Text _achievementText;
    private static AudioClip _inSound, _outSound;

    /* Method for initializing the panel object */
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
        AddAchieveText();
        
        /* Set audio files */
        SetAudioComponents();
    }

    /* Method for setting the panel texture */
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

    /* Method for setting the panel size */
    private static void SetPanelSize() {
        Vector2 size = Chat.instance.m_chatWindow.GetComponent<RectTransform>().sizeDelta;  //Get size of the chat box
        float width = size.x,
              height = width / ASPECT_RATIO;  //Get the width and the height of the panel
        _size = new Vector2(width, height);  //Set the new size
        panelRect.sizeDelta = _size;  //Apply the new size to panel
    }

    /* Method for setting the panel position */
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

    /* Method for adding the header text */
    private static void AddHeaderText() {
        const float offsetX = 1.1f,
                    offsetY = 1.5f;
        
        GameObject textObject = new GameObject("Header_Text", typeof(Text));  //Create a new game object
        textObject.transform.SetParent(_panel.transform);  //Set the achievement panel as the parent for this object
        textObject.transform.localPosition = new Vector3(0f, 0f, 0f);  //Set local position of the text object
        
        Text headerText = textObject.GetComponent<Text>();  //Get text component of this object
        headerText.rectTransform.sizeDelta = panelRect.sizeDelta / new Vector2(offsetX, offsetY);  //Set text size
        AddOutline(textObject);  //Add the outline to the text

        /* Set text properties */
        headerText.text = Localizer.AchievePanelHeader;
        headerText.color = Color.yellow;
        headerText.font = Fonts.Norsebold;
        headerText.fontStyle = FontStyle.Bold;
        headerText.fontSize = 32;
        headerText.alignment = TextAnchor.UpperLeft;
        headerText.horizontalOverflow = HorizontalWrapMode.Overflow;

        /* Setting the text outline */
        Outline headerOutline = textObject.GetComponent<Outline>();  //Get outline component
        headerOutline.effectDistance = new Vector2(1.5f, 1.5f);
        headerOutline.effectColor = Color.black;
        headerOutline.useGraphicAlpha = false;
    }

    /* Method for adding the achievement name text */
    private static void AddAchieveText() {
        const float offsetX = 1.1f,
                    offsetY = 1.8f;
        
        GameObject textObject = new GameObject("Achievement_Text", typeof(Text));  //Create a new game object
        textObject.transform.SetParent(_panel.transform);  //Set the achievement panel as the parent for this object
        textObject.transform.localPosition = new Vector3(0f, 0f, 0f);  //Set local position of the text object
        
        _achievementText = textObject.GetComponent<Text>();  //Get text component of this object
        _achievementText.rectTransform.sizeDelta = panelRect.sizeDelta / new Vector2(offsetX, offsetY);  //Set text size
        _achievementText.rectTransform.position -= new Vector3(0f, _achievementText.rectTransform.sizeDelta.y / offsetY);  //Shift the text object
        AddOutline(textObject);  //Add the outline to the text
        
        /* Set text properties */
        _achievementText.text = "null";
        _achievementText.color = Color.white;
        _achievementText.font = Fonts.AveriaSerifLibre;
        _achievementText.fontStyle = FontStyle.Normal;
        _achievementText.fontSize = 26;
        _achievementText.alignment = TextAnchor.UpperLeft;
        _achievementText.horizontalOverflow = HorizontalWrapMode.Overflow;
    }
    
    /* Method for adding the outline to Unity object
     * gameObject - the game object to add an outline to */
    private static void AddOutline(GameObject gameObject) {
        Outline headerOutline = gameObject.AddComponent<Outline>();
        headerOutline.effectDistance = new Vector2(1.5f, 1.5f);
        headerOutline.effectColor = Color.black;
        headerOutline.useGraphicAlpha = false;
    }

    private static void SetAudioComponents() {
        const string mainNamespace = "AwesomeAchievements.Assets.Sounds",
                     inSoundName = "AchievementPanelIn.ogg",
                     outSoundName = "AchievementPanelOut.ogg";
        
        /* Init a new audio source */
        GameObject audioObject = new GameObject("AchievementSounds");  //Create a new object
        audioSource = audioObject.AddComponent<AudioSource>();  //Add an audio source component
        audioSource.volume = 1f;  //Set volume
        audioObject.transform.parent = AudioMan.instance.transform;  //Set the audio manager as the parent object
        
        /* Get "in" panel sound */
        ResourceReader soundReader = new ResourceReader($"{mainNamespace}.{inSoundName}");
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file:///" + soundReader.WriteToTmp(inSoundName), AudioType.OGGVORBIS);
        request.SendWebRequest();
        WaitForRequest();
        _inSound = DownloadHandlerAudioClip.GetContent(request);

        /* Get "out" panel sound */
        soundReader = new ResourceReader($"{mainNamespace}.{outSoundName}");
        request = UnityWebRequestMultimedia.GetAudioClip("file:///" + soundReader.WriteToTmp(outSoundName), AudioType.OGGVORBIS);
        request.SendWebRequest();
        WaitForRequest();
        _outSound = DownloadHandlerAudioClip.GetContent(request);

        void WaitForRequest() {
            while (!request.isDone) Thread.Sleep(100);
        }
    }

    public static void PlayInSound() {
        audioSource.clip = _inSound;
        audioSource.Play();
    }

    public static void PlatOutSound() {
        audioSource.clip = _outSound;
        audioSource.Play();
    }

    /* Method for showing the achievement panel */
    public static void ShowPanel(string achievementName) {
        if (_panel.isBusy) {
            Queue.Add(achievementName);
            return;
        }

        _achievementText.text = achievementName;
        _panel.Appear();
    }

    /* Method for run the next pended action (if it exists)
     * (User can complete more than one achievement in a short time, so we need to pend the showing of the panel) */
    public static void NextPendedAction() {
        if (_panel.isBusy) return;
        if (Queue.Count == 0) return;

        string achievementName = Queue[0];
        Queue.RemoveAt(0);
        
        ShowPanel(achievementName);
    }
}