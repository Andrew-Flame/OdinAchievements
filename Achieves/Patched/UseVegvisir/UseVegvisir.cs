namespace VikingAchievements.Achieves.Patched.UseVegvisir;

internal sealed partial class UseVegvisir : SimpleAchieve { 
    public UseVegvisir(string name, string description) : base(name, description) { }
    
    protected override void InitPatchers() => AddPatcher<VegvisirInteract>();
}