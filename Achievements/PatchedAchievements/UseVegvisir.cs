using AwesomeAchievements.Patches;

namespace AwesomeAchievements.Achievements.PatchedAchievements; 

internal class UseVegvisir : SingleAchievement {
    public UseVegvisir(string name, string description) : base(name, description) {
        AddPatcher(new VegvisirInteract());
        PatchAll();
    }
}

[AchievementPatch(typeof(Vegvisir), "Interact")]
file class VegvisirInteract : Patcher {
    public static void Postfix() {
        AchievementsContainer.CompleteAchievement("UseVegvisir");
    }
}