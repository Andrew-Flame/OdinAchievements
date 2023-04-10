namespace AwesomeAchievements.Achieves; 

internal abstract class LengthyAchievement : Achievement {
    public LengthyAchievement(string name, string description) : base(name, description) { }

    public override byte[] SavingData() {
        throw new System.NotImplementedException();
    }
}