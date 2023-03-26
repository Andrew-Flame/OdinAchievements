// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace AwesomeAchievements.AchievementsLists; 

internal struct AchievementJsonObject {
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
}

internal struct AchievementJsonArray {
    public AchievementJsonObject[] data { get; set; }
}