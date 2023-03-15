using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using AwesomeAchievements.AchievementsList;

namespace Tests; 

internal static class AchievementsListTest {
    public static void Run() {
        System.Console.WriteLine("Achievements list test:");
        DirectoryInfo dir = new DirectoryInfo("../../../AchievementsList");  //Get directory with json documents
        var files = dir.GetFiles("*.json");  //Get all json documents from this directory
        FileInfo templateFile = (from file in files where file.Name.Contains("template") select file).First();  //Get template file from this files
        string templateJson = File.ReadAllText(templateFile.FullName);  //Read data from template file
        
        for (ushort i = 0; i < files.Length; i++) {
            FileInfo file = files[i];  //Get a fileInfo from array
            if (file.Name.Contains("template")) continue;  //If it is the template file, go to the next iteration
            string fileJson = File.ReadAllText(file.FullName);  //Read the data from file

            /* Create objects using json data from files */
            var templateData = JsonConvert.DeserializeObject<AchievementJsonArray>(templateJson).data;
            var fileData = JsonConvert.DeserializeObject<AchievementJsonArray>(fileJson).data;
            
            if (Eval(templateData, fileData)) {  //Check the data for correctness
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("[✔] Test #" + (i + 1) + $" ({file.Name}) passed");
            } else {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("[ ] Test #" + (i + 1) + $" ({file.Name}) failed");
            }
        }
        
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine();
    }

    private static bool Eval(AchievementJsonObject[] templateData, AchievementJsonObject[] fileData) {
        if (templateData.Length != fileData.Length) return false;  //If files have different lenght return false

        /* Get enumerators from arrays */
        var templateDataEnum = templateData.GetEnumerator();
        var fileDataEnum = fileData.GetEnumerator();
        
        /* Compare data using enums */
        for (ushort i = 0; i < templateData.Length; i++) {
            templateDataEnum.MoveNext();
            fileDataEnum.MoveNext();

            var templateObj = (AchievementJsonObject)templateDataEnum.Current!;
            var fileObj = (AchievementJsonObject)fileDataEnum.Current!;

            if (templateObj.id != fileObj.id || fileObj.name is null or "" || fileObj.description is null or "") return false;
        }

        return true;
    }
}