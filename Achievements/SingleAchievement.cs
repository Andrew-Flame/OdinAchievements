namespace AwesomeAchievements.Achievements; 

internal abstract class SingleAchievement : Achievement {
    protected SingleAchievement(string name, string description) : base(name, description) { }

    public override void LoadData(string data) {
        throw new System.NotImplementedException();
    }
}