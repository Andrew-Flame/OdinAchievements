using System;
using System.Reflection;

namespace AwesomeAchievements.Patch; 

/* A class-attribute for convenient work with patches */
internal sealed class AchievePatch : Attribute {
    private readonly Type _classType;
    private readonly string _methodName;
    
    public MethodBase MethodBase => _classType.GetMethod(_methodName);

    /* Constructor for creating an attribute of patcher class for work with harmony patches
     * classType - the type of the patcher class
     * methodName - the name of the method for patching */
    public AchievePatch(Type classType, string methodName) {
        _classType = classType;
        _methodName = methodName;
    }
}