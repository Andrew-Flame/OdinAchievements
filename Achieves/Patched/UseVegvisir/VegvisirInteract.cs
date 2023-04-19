using VikingAchievements.Patch;
using VikingAchievements.Utility;

namespace VikingAchievements.Achieves.Patched.UseVegvisir; 

internal sealed partial class UseVegvisir { 
    [AchievePatch(typeof(Vegvisir), "Interact")] 
    private sealed class VegvisirInteract : Patcher {
        public static void Postfix() {
            AchievesContainer.Get("UseVegvisir").Complete();
        }
    }
}