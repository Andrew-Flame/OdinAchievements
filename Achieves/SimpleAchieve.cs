using VikingAchievements.Saving;

namespace VikingAchievements.Achieves; 

internal abstract class SimpleAchieve : Achievement {
    protected SimpleAchieve(string name, string description) : base(name, description) { }
    
    public override byte[] SavingData() {
        string savingData = $"{Id}{SaveManager.ACHIEVE_SEPARATOR.Repeat()}";
        return savingData.ToByteArray();
    }
}