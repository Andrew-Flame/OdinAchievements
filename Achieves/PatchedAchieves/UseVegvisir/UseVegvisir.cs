namespace AwesomeAchievements.Achieves.PatchedAchieves.UseVegvisir;

internal sealed class UseVegvisir : SingleAchievement { 
    public UseVegvisir(string name, string description) : base(name, description) { }
    
    protected override void InitPatchers() => AddPatcher(new VegvisirInteract());
}