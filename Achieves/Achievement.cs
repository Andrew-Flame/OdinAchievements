// ReSharper disable VirtualMemberCallInConstructor
using System;
using AwesomeAchievements.AchievePanel;
using AwesomeAchievements.Patch;
using AwesomeAchievements.Utility;

namespace AwesomeAchievements.Achieves;

internal abstract class Achievement {
    public string Id  => GetType().Name;
    public string Name { get; }
    public string Description { get; }
    
    private Patcher[] _patchers;

    protected Achievement(string name, string description) {
        Name = name;
        Description = description;
        _patchers = new Patcher[1];
        InitPatchers();
        PatchAll();
    }

    ~Achievement() => UnpatchAll();

    protected abstract void InitPatchers();
    
    protected void AddPatcher<T>() where T: Patcher, new() {
        Patcher patcher = new T();
        if (_patchers.Length == 1) _patchers[0] = patcher;
        else {
            Array.Resize(ref _patchers, _patchers.Length + 1);
            _patchers[_patchers.Length - 1] = patcher;
        }
    }

    public void PatchAll() {
        foreach (Patcher patcher in _patchers) patcher?.Patch();
    }

    public void UnpatchAll() {
        foreach (Patcher patcher in _patchers) patcher?.Unpatch();
    }
    
    public abstract byte[] SavingData();

    public void Complete() {
        LogInfo.Log($"The achievement '{Id}' has been completed");
        PanelManager.ShowPanel(Name);  //Show the achievement panel
        Announcer.Announce(Name);  //Announce the getting of the achievement into the game chat
        UnpatchAll();  //Unpatch all of the patches
        
        /* Remove the achievement from the container */
        AchievesContainer.Remove(Id);
    }
}