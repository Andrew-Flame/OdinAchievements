using System;
using System.Collections.Generic;
using System.Linq;
using VikingAchievements.AchieveLists;
using VikingAchievements.Achieves;
using UnityEngine;

namespace VikingAchievements.Utility; 

/* Container realizing assess for all achievements patches */
internal static class AchievesContainer {
    private static Achievement[] _data;
    
    /* Method for initializing this type
     * can throw an exception if there no at least one achievement class for patch */
    public static void Init() {
        if (_data != null) SafeClear();  //If the container has been initialized, safe clear it
        var achieveList = JsonAchieves(out int lenght);  //Get list of achievement json objects
        _data = new Achievement[lenght];  //Initialize an array of achievements
        const string classesNamespace = "VikingAchievements.Achieves.Patched";  //Namespace where classes contained

        ushort counter = 0;  //Init the counter
        foreach (AchieveJson achieveJson in achieveList) {
            Type achieveClass = Type.GetType($"{classesNamespace}.{achieveJson.Id}.{achieveJson.Id}"); //Get type of the achievement class
            if (achieveClass == null) throw new UnityException($"Class of the achievement \"{achieveJson.Id}\" not found");  //Check class for null
            Achievement achievement = 
                (Achievement)Activator.CreateInstance(achieveClass, achieveJson.Name, achieveJson.Description); //Get instance of the achievement class
            _data[counter++] = achievement;  //Add the achievement to array
        }
        
        LogInfo.Log("An achievement container has been initialized");
    }

    /* Method for getting an achievement by its id from container
     * id - the id of the achievement that we need to get
     * returns the instance of the achievement
     * can throw an exception if there no achievement with the same id in the container */
    public static Achievement Get(string id) {
        foreach (Achievement achievement in _data)  //Cycle through the container data
            if (achievement.Id == id) return achievement;  //Get the achievement with the same id
        
        /* If the achievement with the same id isn't in the container */
        throw new UnityException("Error while getting achievement from container");
    }

    /* Method for completing the achievement by its id
     * id - the id of the achievement that we need to complete */
    public static void Complete(string id) => Get(id).Complete();

    /* Method for deleting an achievement by its id from container
     * id - the id of the achievement that we need to delete
     * can throw an exception if there no achievement with the same id in the container */
    public static void Remove(string id) {
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

    /* Method for checking has this container the achievement with the same id, or not
     * id - the id of the achievement for checking
     * out achievement - the achievement with the same id
     * returns the true - id this container has an achievement with the same id, otherwise - false */
    public static bool Has(string id, out Achievement achievement) {
        achievement = _data.FirstOrDefault(e => e.Id == id);
        return achievement != null;
    }

    /* Method for getting the array of the achievement json objects
     * language - the language in which the achievements should be
     * returns the array with achievement json objects containing their ids, names and descriptions */
    public static IEnumerable<AchieveJson> JsonAchieves(out int lenght) {
        string listPath = $"Achieves.Lists.{ConfigValues.Language}.min.json";  //Get a path of the list resource
        ResourceReader listReader = new ResourceReader(listPath);  //Create a resource reader for a json list
        var jsonParser = new JsonParser(listReader.ReadAllStrings());  //Create an instance of the JSON  parser
        lenght = jsonParser.AchievesCount;  //Get the lenght of the sequence
        return jsonParser.ParseAchieves();  //Return deserialized json data
    }

    /* Method for the save clearing the container from patched achievements */
    private static void SafeClear() {
        foreach (Achievement achievement in _data) achievement.UnpatchAll();  //Unpatch all achievements
        _data = null;  //Remove the array reference
    }
}