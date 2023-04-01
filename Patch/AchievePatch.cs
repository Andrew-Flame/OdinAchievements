using System;
using System.Reflection;

namespace AwesomeAchievements.Patch; 

/// <summary>Class-attribute for convenient work with patches</summary>
internal sealed class AchievePatch : Attribute {
    private readonly Type _classType;
    private readonly string _methodName;
    
    /// <summary>Get MethodBase for work with harmony</summary>
    public MethodBase MethodBase => _classType.GetMethod(_methodName);

    public AchievePatch(Type classType, string methodName) {
        _classType = classType;
        _methodName = methodName;
    }
}