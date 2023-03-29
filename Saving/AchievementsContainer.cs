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
    public static Achievement[] data { get; private set; }
    
    public AchievementsContainer([NotNull]string language) {
        var achievementList = GetAchievementList(language);
        data = new Achievement[achievementList.Length];

        const string classesNamespace = "AwesomeAchievements.Achievements.PatchedAchievements";  //Namespace where classes contained
        for (var i = 0; i < achievementList.Length; i++) {
            var achievementJson = achievementList[i];
            Type achievementClass = Type.GetType($"{classesNamespace}.{achievementJson.id}"); //Get type of the achievement class
            Achievement achievement = (Achievement)Activator
                .CreateInstance(achievementClass ?? throw new UnityException("Class of the achievement \"" + achievementJson.id + "\" not found"),
                                achievementJson.name, achievementJson.description); //Get instance of the achievement class
            data[i] = achievement;  //Add the achievement to array
        }
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