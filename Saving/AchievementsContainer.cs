using System;
using System.IO;
using System.Reflection;
using AwesomeAchievements.AchievementLists;
using AwesomeAchievements.Achievements;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace AwesomeAchievements.Saving; 

internal sealed class AchievementsContainer {
    private Achievement[] _data;
    
    public AchievementsContainer([NotNull]string language) {
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

    /// <summary>Get the achievement by id from the container</summary>
    /// <param name="id">Id of the achievement</param>
    /// <returns>Achievement</returns>
    /// <exception cref="UnityException">If the container doesn't contain the achievement with the same id</exception>
    public Achievement GetAchievement(string id) {
        foreach (Achievement achievement in _data) 
            if (achievement.Id == id) return achievement;
        throw new UnityException("Error while getting achievement from container");
    }
    
    /// <summary>Delete the achievement by id from the container</summary>
    /// <param name="id">Id of the achievement</param>
    /// <exception cref="UnityException">If the container doesn't contain the achievement with the same id</exception>
    public void DeleteAchievement(string id) {
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

    /// <summary>Make the achievement by id completed</summary>
    /// <param name="id">Id of the achievement</param>
    public void CompleteAchievement(string id) {
        Achievement achievement = GetAchievement(id);
        achievement.UnpatchAll();
        achievement.Complete();
    } 

    /// <summary>Get list of achievements from embedded resources</summary>
    /// <param name="language">Name of achievement list</param>
    /// <returns>List of achievements</returns>
    private static AchievementJsonObject[] GetAchievementList([NotNull]string language) {
        Assembly assembly = Assembly.GetExecutingAssembly();  //Get executing assembly
        const string resourceNamespace = "AwesomeAchievements.AchievementLists";  //Namespace which contains lists of achievements
        
        /* Read required list */
        Stream listStream = assembly.GetManifestResourceStream($"{resourceNamespace}.{language}.json");
        StreamReader listReader = new StreamReader(listStream!);
        var result = JsonConvert.DeserializeObject<AchievementJsonArray>(listReader.ReadToEnd()).data;
        
        /* Close streams */
        listStream.Close();
        listReader.Close();

        return result;  //Return the result
    }
}