namespace AwesomeAchievements.Achieves; 

internal abstract class HardAchieve : Achievement {
    protected HardAchieve(string name, string description) : base(name, description) { }
    
    public override byte[] SavingData() {
        throw new System.NotImplementedException();
    }
}