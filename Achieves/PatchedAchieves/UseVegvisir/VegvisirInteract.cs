using AwesomeAchievements.Patch;
using AwesomeAchievements.Utility;

namespace AwesomeAchievements.Achieves.PatchedAchieves.UseVegvisir; 

[AchievePatch(typeof(Vegvisir), "Interact")] 
internal sealed class VegvisirInteract : Patcher {
    public static void Postfix() {
        AchievesContainer.Get("UseVegvisir").Complete();
    }
}