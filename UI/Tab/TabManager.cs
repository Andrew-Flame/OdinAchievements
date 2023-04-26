using UnityEngine;
using UnityEngine.UI;
using VikingAchievements.UI.Icon;
using VikingAchievements.Utility;

namespace VikingAchievements.UI.Tab; 

internal static class TabManager {
    public static bool isOpened;
    
    private static Transform _parent;
    private static Transform _frame;
    
    public static void Init() {
        Transform rootInventory = InventoryGui.instance.m_inventoryRoot;
        Transform trophiesTab = rootInventory.Find("Trophies");
        _parent = Object.Instantiate(trophiesTab, rootInventory.transform, true);
        _parent.name = "Achievements";
        
        RefactorObject();
        RedefineCloseButtons();
    }

    private static void RefactorObject() {
        _frame = _parent.transform.Find("TrophiesFrame");
        Transform trophies = _frame.Find("Trophies");

        _frame.name = "AchievementFrame";
        trophies.name = "Achievements";

        _frame.Find("topic").GetComponent<Text>().text = Localizer.TabTopic;
        trophies.Find("TrophyListScroll").name = "AchievementListScroll";

        trophies.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.65f);
        Object.Destroy(trophies.Find("TrophyList").gameObject);
    }

    private static void RedefineCloseButtons() {
        _parent.Find("Closebutton").GetComponent<Button>().onClick.AddListener(Close);
        _frame.Find("Closebutton").GetComponent<Button>().onClick.AddListener(Close);
    }

    public static void Show() {
        IconManager.icon.SetFocused();
        isOpened = true;
        _parent.gameObject.SetActive(true);
    }

    public static void Close() {
        IconManager.icon.SetDefault();
        isOpened = false;
        _parent.gameObject.SetActive(false);
    }
}