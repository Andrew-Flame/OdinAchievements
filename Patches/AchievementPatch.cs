using System;
using System.Reflection;

namespace AwesomeAchievements.Patches; 

internal class AchievementPatch : Attribute {
    private readonly Type _classType;
    private readonly string _methodName;
    
    public MethodBase MethodBase => _classType.GetMethod(_methodName);

    public AchievementPatch(Type classType, string methodName) {
        _classType = classType;
        _methodName = methodName;
    }
}