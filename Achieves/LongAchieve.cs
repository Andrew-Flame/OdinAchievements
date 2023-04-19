namespace VikingAchievements.Achieves; 

internal abstract class LongAchieve : Achievement {
    protected LongAchieve(string name, string description) : base(name, description) { }

    public override byte[] SavingData() {
        throw new System.NotImplementedException();
    }
}