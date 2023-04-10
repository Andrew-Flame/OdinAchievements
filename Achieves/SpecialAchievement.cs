namespace AwesomeAchievements.Achieves; 

internal abstract class SpecialAchievement : Achievement {
    public SpecialAchievement(string name, string description) : base(name, description) { }
    
    public override byte[] SavingData() {
        throw new System.NotImplementedException();
    }
}