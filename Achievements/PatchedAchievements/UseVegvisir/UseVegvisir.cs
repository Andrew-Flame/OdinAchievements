namespace AwesomeAchievements.Achievements.PatchedAchievements.UseVegvisir; 

internal class UseVegvisir : SingleAchievement {
    public UseVegvisir(string name, string description) : base(name, description) { }

    protected override void InitPatchers() => AddPatcher(new VegvisirInteract());
}