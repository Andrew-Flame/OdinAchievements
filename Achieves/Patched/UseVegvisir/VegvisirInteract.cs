using AwesomeAchievements.Patch;
using AwesomeAchievements.Utility;

namespace AwesomeAchievements.Achieves.Patched.UseVegvisir; 

internal sealed partial class UseVegvisir { 
    [AchievePatch(typeof(Vegvisir), "Interact")] 
    private sealed class VegvisirInteract : Patcher {
        public static void Postfix() {
            AchievesContainer.Get("UseVegvisir").Complete();
        }
    }
}