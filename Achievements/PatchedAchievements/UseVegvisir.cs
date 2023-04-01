using AwesomeAchievements.Patch;

namespace AwesomeAchievements.Achievements.PatchedAchievements; 

internal class UseVegvisir : SingleAchievement {
    public UseVegvisir(string name, string description) : base(name, description) {
        AddPatcher(new VegvisirInteract());
        PatchAll();
    }
}

[AchievePatch(typeof(Vegvisir), "Interact")]
file sealed class VegvisirInteract : Patcher {
    public static void Postfix() {
        AchieveContainer.GetAchievement("UseVegvisir").Complete();
    }
}