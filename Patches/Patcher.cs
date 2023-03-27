using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace AwesomeAchievements.Patches; 

/// <summary>Abstract class containing methods for quick patch and unpatch harmony patches</summary>
internal abstract class Patcher {
    private AchievementPatch _patch;
    
    /// <summary>Patch methods from this class using harmony</summary>
    /// <exception cref="UnityException">When class hasn't `AchievementPatch` attribute</exception>
    public void Patch() {
        _patch = GetType().GetCustomAttribute<AchievementPatch>();
        if (_patch == null) throw new UnityException("Patcher class hasn't <AchievementPatch> attribute");

        MethodInfo prefix = GetType().GetMethod("Prefix");
        MethodInfo postfix = GetType().GetMethod("Postfix");

        Master.harmony.Patch(_patch.MethodBase, 
                             prefix: prefix == null ? null : new HarmonyMethod(prefix), 
                             postfix: postfix == null ? null : new HarmonyMethod(postfix));
    }

    /// <summary>Unpatch methods from harmony</summary>
    public void Unpatch() => Master.harmony.Unpatch(_patch.MethodBase, HarmonyPatchType.All);

    /// <summary>When collecting garbage, it should be unpatched</summary>
    ~Patcher() => Unpatch();
}