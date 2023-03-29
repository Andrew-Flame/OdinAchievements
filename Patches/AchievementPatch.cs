using System;
using System.Reflection;

namespace AwesomeAchievements.Patches; 

/// <summary>Class-attribute for convenient work with patches</summary>
internal sealed class AchievementPatch : Attribute {
    private readonly Type _classType;
    private readonly string _methodName;
    
    /// <summary>Get MethodBase for work with harmony</summary>
    public MethodBase MethodBase => _classType.GetMethod(_methodName);

    public AchievementPatch(Type classType, string methodName) {
        _classType = classType;
        _methodName = methodName;
    }
}