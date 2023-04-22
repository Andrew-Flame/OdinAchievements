using System.Collections.Generic;
using System.Threading;
using VikingAchievements.Utility;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace VikingAchievements.AchievePanel; 

/* Class for work with the achievement panel */
internal static class PanelManager {
    private const float ASPECT_RATIO = 704f / 174f;
    private static readonly List<string> Queue = new();
    private static AchievePanel _panel;
    private static Text _achieveText;
    private static AudioSource _audioSource;
    private static AudioClip _inSound, _outSound;

    /* Method for initializing the panel object */
    public static void Init() {
        /* If the panel is already initialized */
        if (_panel != null) SafeClear();  //Safe clear game objects
        
        /* Init the achievement panel game object */
        GameObject panel = new GameObject("Achievement_Panel", typeof(Image), typeof(AchievePanel));  //Create an achievement panel object
        panel.transform.SetParent(Hud.instance.transform.parent.transform);  //Set the hud root as the parent for the achievement panel
        //panel.SetActive(false);  //Hide the panel
        
        /* Get panel components */
        _panel = panel.GetComponent<AchievePanel>();  //Get the achieve panel component
        _panel.rect = panel.GetComponent<RectTransform>();  //Get the achievement panel rect transform component

        /* Set achievement panel properties */
        SetPanelTexture();  //Set the achievement panel texture
        SetPanelSize();  //Set achievement panel size
        SetPanelPosition();  //Set achievement panel position
        
        /* Add achievement panel childs */
        AddHeaderText();
        AddAchieveText();
        
        /* Set audio files */
        SetAudioComponents();
        
        LogInfo.Log("An achievement panel manager has been initialized");
    }
    
    /* Method for safe clearing game objects */
    private static void SafeClear() {
        Object.Destroy(_panel.gameObject);
        Object.Destroy(_audioSource.gameObject);
    }

    /* Method for setting the panel texture */
    private static void SetPanelTexture() {
        Image panelImage = _panel.GetComponent<Image>();  //Get an image component from the achievement panel
        ResourceReader resourceReader = new ResourceReader("Assets.Textures.AchievementPanel.png");  //Get a resource from assembly
        panelImage.sprite = resourceReader.GetSprite();  //Set the new sprite
        panelImage.mainTexture.wrapMode = TextureWrapMode.Clamp;  //Set the wrap mode to clamp
    }

    /* Method for setting the panel size */
    private static void SetPanelSize() {
        Vector2 size = Chat.instance.m_chatWindow.GetComponent<RectTransform>().sizeDelta;  //Get size of the chat box
        float width = size.x,
              height = width / ASPECT_RATIO;  //Get the width and the height of the panel
        Vector2 newSize = new Vector2(width, height);  //Set the new size
        _panel.rect.sizeDelta = newSize;  //Apply the new size to panel
    }

    /* Method for setting the panel position */
    private static void SetPanelPosition() {
        /* Set anchors to the top right screen corner */
        _panel.rect.anchorMin = new Vector2(1f, 1f);
        _panel.rect.anchorMax = new Vector2(1f, 1f);

        /* Set the new panel position */
        var minimapVectors = new Vector3[4];  //Init the array of minimap vectors
        Minimap.instance.m_smallRoot.GetComponent<RectTransform>().GetWorldCorners(minimapVectors);  //Get minimap corners coordinates
        _panel.rect.pivot = new Vector2(1f, 1f);  //Set the pivot of the panel to top right corner
        _panel.rect.position = new Vector3(Screen.width - 5f, minimapVectors[0].y);  //Set the correct panel position
    }

    /* Method for adding the header text */
    private static void AddHeaderText() {
        const float offsetX = 1.115f,
                    offsetY = 1.5f;
        
        GameObject textObject = new GameObject("Header_Text", typeof(Text));  //Create a new game object
        textObject.transform.SetParent(_panel.transform);  //Set the achievement panel as the parent for this object
        textObject.transform.localPosition = new Vector3(0f, 0f, 0f);  //Set local position of the text object
        
        /* Change the anchored position */
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchoredPosition = new Vector2(0f, 0f);
        
        Text headerText = textObject.GetComponent<Text>();  //Get text component of this object
        headerText.rectTransform.sizeDelta = _panel.rect.sizeDelta / new Vector2(offsetX, offsetY);  //Set text size
        AddOutline(textObject);  //Add the outline to the text

        /* Set text properties */
        headerText.text = Localizer.AchievePanelHeader;
        headerText.color = new Color(1f, 0.7f, 0f);
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
        const float offsetX = 1.115f,
                    offsetY = 1.75f;
        
        GameObject textObject = new GameObject("Achievement_Text", typeof(Text));  //Create a new game object
        textObject.transform.SetParent(_panel.transform);  //Set the achievement panel as the parent for this object
        textObject.transform.localPosition = new Vector3(0f, 0f, 0f);  //Set local position of the text object
        
        /* Change the anchored position */
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchoredPosition = new Vector2(0f, 0f);
        
        _achieveText = textObject.GetComponent<Text>();  //Get text component of this object
        _achieveText.rectTransform.sizeDelta = _panel.rect.sizeDelta / new Vector2(offsetX, offsetY);  //Set text size
        _achieveText.rectTransform.position -= new Vector3(0f, _achieveText.rectTransform.sizeDelta.y / offsetY);  //Shift the text object
        AddOutline(textObject);  //Add the outline to the text
        
        /* Set text properties */
        _achieveText.text = "null";
        _achieveText.color = Color.white;
        _achieveText.font = Fonts.AveriaSerifLibre;
        _achieveText.fontStyle = FontStyle.Normal;
        _achieveText.fontSize = 26;
        _achieveText.alignment = TextAnchor.UpperLeft;
        _achieveText.horizontalOverflow = HorizontalWrapMode.Overflow;
    }
    
    /* Method for adding the outline to Unity object
     * gameObject - the game object to add an outline to */
    private static void AddOutline(GameObject gameObject) {
        Outline headerOutline = gameObject.AddComponent<Outline>();
        headerOutline.effectDistance = new Vector2(1f, 1f);
        headerOutline.effectColor = Color.black;
        headerOutline.useGraphicAlpha = false;
    }

    /* Method for setting audio components */
    private static void SetAudioComponents() {
        const string mainNamespace = "Assets.Sounds",
                     inSoundName = "AchievementPanelIn.ogg",
                     outSoundName = "AchievementPanelOut.ogg";
        
        /* Init a new audio source */
        GameObject audioObject = new GameObject("AchievementSounds");  //Create a new object
        _audioSource = audioObject.AddComponent<AudioSource>();  //Add an audio source component
        audioObject.transform.parent = AudioMan.instance.transform;  //Set the audio manager as the parent object
        
        /* Setting the audio source */
        _audioSource.volume = ConfigValues.Volume / 100f;
        _audioSource.spatialize = false;
        _audioSource.spatializePostEffects = false;
        _audioSource.spatialBlend = 0f;
        
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

    /* Method for playing the "panel appearing" sound */
    public static void PlayInSound() {
        _audioSource.clip = _inSound;
        _audioSource.Play();
    }

    /* Method for playing the "panel disappearing" sound */
    public static void PlatOutSound() {
        _audioSource.clip = _outSound;
        _audioSource.Play();
    }

    /* Method for showing the achievement panel */
    public static void ShowPanel(string achievementName) {
        if (_panel.isBusy) {  //If the panel is busy
            Queue.Add(achievementName);  //Add the achievement name to the queue
            return;  //And exit the method
        }
        
        /* Else show the popup panel */
        _achieveText.text = achievementName;
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