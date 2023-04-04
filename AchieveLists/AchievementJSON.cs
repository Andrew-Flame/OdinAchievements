// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace AwesomeAchievements.AchieveLists; 

/* Class describing a Json achievement object from lists */
internal struct AchievementJsonObject {
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
}

/* Class describing a Json achievement array from lists */
internal struct AchievementJsonArray {
    public AchievementJsonObject[] data { get; set; }
}