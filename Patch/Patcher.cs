using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace AwesomeAchievements.Patch; 

internal abstract class Patcher {
    private AchievePatch _patch;
    
    public void Patch() {
        _patch = GetType().GetCustomAttribute<AchievePatch>();
        if (_patch == null) throw new UnityException("Patcher class hasn't <AchievementPatch> attribute");

        MethodInfo prefix = GetType().GetMethod("Prefix");
        MethodInfo postfix = GetType().GetMethod("Postfix");

        Master.harmony.Patch(_patch.MethodBase,
                             prefix: prefix == null ? null : new HarmonyMethod(prefix), 
                             postfix: postfix == null ? null : new HarmonyMethod(postfix));
    }
    
    public void Unpatch() => Master.harmony.Unpatch(_patch.MethodBase, HarmonyPatchType.All);
    
    ~Patcher() => Unpatch();
}