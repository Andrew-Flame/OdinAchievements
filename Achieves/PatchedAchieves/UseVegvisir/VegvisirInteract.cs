using AwesomeAchievements.Patch;

namespace AwesomeAchievements.Achieves.PatchedAchieves.UseVegvisir; 

[AchievePatch(typeof(Vegvisir), "Interact")] 
internal sealed class VegvisirInteract : Patcher {
    public static void Postfix() {
        AchieveContainer.GetAchievement("UseVegvisir").Complete();
    }
}