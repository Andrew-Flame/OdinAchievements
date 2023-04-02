// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace AwesomeAchievements.AchievementLists; 

/// <summary>Class describing a Json achievement object from lists</summary>
internal struct AchievementJsonObject {
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
}

/// <summary>Class describing a Json achievement array from lists</summary>
internal struct AchievementJsonArray {
    public AchievementJsonObject[] data { get; set; }
}