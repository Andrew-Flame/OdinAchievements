using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace AwesomeAchievements.Patches; 

internal class Patcher {
    private AchievementPatch _patch;
    
    public void Patch() {
        _patch = GetType().GetCustomAttribute<AchievementPatch>();
        if (_patch == null) throw new UnityException("Patcher class has not <AchievementPatch> attribute");

        MethodInfo prefix = GetType().GetMethod("Prefix");
        MethodInfo postfix = GetType().GetMethod("Postfix");

        Master.harmony.Patch(_patch.MethodBase, 
                             prefix: prefix == null ? null : new HarmonyMethod(prefix), 
                             postfix: postfix == null ? null : new HarmonyMethod(postfix));
    }

    public void Unpatch() => Master.harmony.Unpatch(_patch.MethodBase, HarmonyPatchType.All);
}