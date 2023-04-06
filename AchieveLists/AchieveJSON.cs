using UnityEngine;

namespace AwesomeAchievements.AchieveLists; 

/* Class describing a Json achievement object from lists */
internal readonly struct AchieveJson {
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }

    public AchieveJson() => throw new UnityException("Can't create an 'AchieveJson' object instance from default constructor");
    public AchieveJson(string id, string name, string description) {
        Id = id;
        Name = name;
        Description = description;
    }
}