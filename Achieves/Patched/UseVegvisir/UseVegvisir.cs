namespace AwesomeAchievements.Achieves.Patched.UseVegvisir;

internal sealed class UseVegvisir : SimpleAchieve { 
    public UseVegvisir(string name, string description) : base(name, description) { }
    
    protected override void InitPatchers() => AddPatcher<VegvisirInteract>();
}