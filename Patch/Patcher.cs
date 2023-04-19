using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace VikingAchievements.Patch; 

/* Class for work with achievement patches
 * it helps to patch and unpatch single harmony patches */
internal abstract class Patcher {
    private AchievePatch _patch;
    
    /* Method for patching the harmony patch
     * can throw an exception if this class hasn't <AchievePatch> attribute */
    public void Patch() {
        _patch = GetType().GetCustomAttribute<AchievePatch>();  //Get the attribute class of this class
        if (_patch == null) throw new UnityException("Patcher class hasn't <AchievementPatch> attribute");  //If there is no attribute, throw an exception

        MethodInfo prefix = GetType().GetMethod("Prefix");  //Get prefix method
        MethodInfo postfix = GetType().GetMethod("Postfix");  //Get postfix method

        Master.harmony.Patch(_patch.MethodBase,
                             prefix: prefix == null ? null : new HarmonyMethod(prefix), 
                             postfix: postfix == null ? null : new HarmonyMethod(postfix));  //Patch all methods which are not null
    }
    
    /* Method for unpatching the harmony patch */
    public void Unpatch() => Master.harmony.Unpatch(_patch.MethodBase, HarmonyPatchType.All);
    
    /* Destructor automatically unpatching the harmony patch */
    ~Patcher() => Unpatch();
}