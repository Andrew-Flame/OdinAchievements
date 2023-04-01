using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAchievements.AchievementLists;
using AwesomeAchievements.Achievements;
using AwesomeAchievements.Utility;
using Newtonsoft.Json;
using UnityEngine;

namespace AwesomeAchievements; 

internal static class AchieveContainer {
    private static Achievement[] _data;
    
    public static void Init(string language) {
        var achievementList = GetAchievementList(language);
        _data = new Achievement[achievementList.Length];

        const string classesNamespace = "AwesomeAchievements.Achievements.PatchedAchievements";  //Namespace where classes contained
        for (ushort i = 0; i < achievementList.Length; i++) {
            var achievementJson = achievementList[i];
            Type achievementClass = Type.GetType($"{classesNamespace}.{achievementJson.id}"); //Get type of the achievement class
            Achievement achievement = (Achievement)Activator
                .CreateInstance(achievementClass ?? throw new UnityException("Class of the achievement \"" + achievementJson.id + "\" not found"),
                                achievementJson.name, achievementJson.description); //Get instance of the achievement class
            _data[i] = achievement;  //Add the achievement to array
        }
    }

    public static IEnumerable<Achievement> GetAllAchieves() => _data.Select(e => e);
    
    public static Achievement GetAchievement(string id) {
        foreach (Achievement achievement in _data) 
            if (achievement.Id == id) return achievement;
        throw new UnityException("Error while getting achievement from container");
    }

    public static void DeleteAchievement(string id) {
        var newData = new Achievement[_data.Length - 1];  //Create new array with a less lenght
        
        /* Cycle trough the array with achievements */
        bool flag = false;
        for (ushort i = 0; i < newData.Length; i++) {
            if (_data[i].Id != id && !flag) {  //Until find an achievement with the same id, just fill in a new array
                newData[i] = _data[i];
                continue;
            }

            /* When find such achievement */
            if (_data[i].Id == id) {
                flag = true;  //Raise the flag
                _data[i].UnpatchAll();  //Unpatch all patches
            }
            
            newData[i] = _data[i + 1];  //Then fill in the array with an offset to the left
        }

        /* Throw an exception if the container doesn't contain the achievement with the same id */
        if (!flag && _data[_data.Length - 1].Id != id)
            throw new UnityException("Can't delete an achievement with the same id from the container");
        
        _data = newData;  //Change the reference of the old array to the new array
    }
    
    public static AchievementJsonObject[] GetAchievementList(string language) {
        /* Read required list */
        const string resourceNamespace = "AwesomeAchievements.AchievementLists";
        ResourceReader listReader = new ResourceReader($"{resourceNamespace}.{language}.json");
        var result = JsonConvert.DeserializeObject<AchievementJsonArray>(listReader.ReadString()).data;

        return result;
    }
}