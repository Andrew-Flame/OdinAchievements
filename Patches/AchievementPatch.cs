using System;
using System.Reflection;

namespace AwesomeAchievements.Patches; 

/// <summary>Class-attribute for convenient work with patches</summary>
internal abstract class AchievementPatch : Attribute {
    private readonly Type _classType;
    private readonly string _methodName;
    
    /// <summary>Get MethodBase for work with harmony</summary>
    public MethodBase MethodBase => _classType.GetMethod(_methodName);

    protected AchievementPatch(Type classType, string methodName) {
        _classType = classType;
        _methodName = methodName;
    }
}