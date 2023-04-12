using AwesomeAchievements.Saving;

namespace AwesomeAchievements.Achieves; 

internal abstract class SingleAchievement : Achievement {
    protected SingleAchievement(string name, string description) : base(name, description) { }
    
    public override byte[] SavingData() {
        string savingData = $"{Id}{SaveManager.ACHIEVE_SEPARATOR.Repeat()}";
        return savingData.ToByteArray();
    }
}