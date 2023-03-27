using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AwesomeAchievements.AchievementLists;
using Newtonsoft.Json;

namespace Tests; 

internal static class AchievementsListTest {
    public static void Run() {
        System.Console.WriteLine("Achievements list test:");
        
        Assembly assembly = Assembly.GetAssembly(typeof(AwesomeAchievements.Master));
        const string resourceNamespace = "AwesomeAchievements.AchievementLists.",
                     templateResourcePath = resourceNamespace + "template.json";
        
        var resources = from resource in assembly.GetManifestResourceNames()
                        where resource.StartsWith(resourceNamespace) && resource != templateResourcePath
                        select resource;

        string templateJson;
        int counter = 0;
        using (Stream templateStream = assembly.GetManifestResourceStream(templateResourcePath))
        using (StreamReader templateReader = new StreamReader(templateStream!)) {
            templateJson = templateReader.ReadToEnd();
        }
        
        foreach (string resourcePath in resources) {
            string resourceJson;
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader resourceReader = new StreamReader(resourceStream!)) {
                resourceJson = resourceReader.ReadToEnd();
            }

            /* Create objects using json data from files #1# */
            var templateData = JsonConvert.DeserializeObject<AchievementJsonArray>(templateJson).data;
            var fileData = JsonConvert.DeserializeObject<AchievementJsonArray>(resourceJson).data;

            if (Eval(templateData, fileData)) {  //Check the data for correctness
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("[✔] Test #" + (++counter) + $" ({resourcePath.Split('.')[2]}) passed");
            } else {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("[ ] Test #" + (++counter) + $" ({resourcePath.Split('.')[2]}) failed");
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