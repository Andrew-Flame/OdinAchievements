namespace AwesomeAchievements.Achievements; 

internal abstract class SpecialAchievement : Achievement {
    public SpecialAchievement(string name, string description) : base(name, description) { }
    
    public override void LoadData(string data) {
        throw new System.NotImplementedException();
    }
}