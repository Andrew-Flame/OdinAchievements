namespace AwesomeAchievements.Achievements; 

internal abstract class LengthyAchievement : Achievement {
    public LengthyAchievement(string name, string description) : base(name, description) { }
    
    public override void LoadData(string data) {
        throw new System.NotImplementedException();
    }
}