// ReSharper disable VirtualMemberCallInConstructor

using System;
using AwesomeAchievements.AchievePanel;
using AwesomeAchievements.Patch;
using UnityEngine;

namespace AwesomeAchievements.Achievements;

/// <summary>Abstract class describing all kinds of achievements </summary>
internal abstract class Achievement {
    private Patcher[] _patchers;

    public string Id  => GetType().Name;
    public string Name { get; }
    public string Description { get; }

    public Achievement(string name, string description) {
        Name = name;
        Description = description;
        _patchers = new Patcher[1];
    }

    ~Achievement() => UnpatchAll();

    public abstract void LoadData(string data);

    protected void AddPatcher(Patcher patcher) {
        if (_patchers.Length == 1) _patchers[0] = patcher;
        else {
            Array.Resize(ref _patchers, _patchers.Length + 1);
            _patchers[_patchers.Length - 1] = patcher;
        }
    }

    public void PatchAll() {
        foreach (Patcher patcher in _patchers) patcher.Patch();
    }

    public void UnpatchAll() {
        foreach (Patcher patcher in _patchers) patcher.Unpatch();
    }

    public void Complete() {
        UnpatchAll();
        PanelHandler.ShowPanel(Name);
        AchieveContainer.DeleteAchievement(Id);
        Debug.Log($"Achievement \"{Id}\" have been completed");
    }
}