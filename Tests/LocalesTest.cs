using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using VikingAchievements.Utility;

namespace Tests; 

internal static class LocalesTest {
    public static void Run() {
        System.Console.WriteLine("Locales test:");
        
        Assembly assembly = Assembly.GetAssembly(typeof(VikingAchievements.Master));
        const string localesNamespace = "Locales",
                     templateResource = $"{localesNamespace}.template.ini";
        
        var resources = from resource in assembly.GetManifestResourceNames()
                        where resource.Contains(localesNamespace) && !resource.Contains(templateResource)
                        select resource;

        ResourceReader templateReader = new ResourceReader(templateResource, assembly);
        string[] templateData = ClearResource(templateReader.ReadAllStrings());

        ushort counter = 0;
        foreach (string resourcePath in resources) {
            ResourceReader resourceReader = new ResourceReader(resourcePath, assembly);
            string[] resourceData = ClearResource(resourceReader.ReadAllStrings());
            
            if (Eval(templateData, resourceData)) {
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("[✔] Test #" + (++counter) + $" ({resourcePath.Split('.')[2]}) passed");
            } else {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("[ ] Test #" + (++counter) + $" ({resourcePath.Split('.')[2]}) failed");
            }
        }
    }

    private static string[] ClearResource(string data) {
        Regex clearRegex = new Regex(@"((?<==)\s*|\s*(?==)|#.*\n|^\s*\n)");
        Regex getRequired = new Regex(@".+=.+");
        string cleared = clearRegex.Replace(data, string.Empty);
        MatchCollection matches = getRequired.Matches(cleared);
        string[] result = new string[matches.Count];

        for (ushort i = 0; i < matches.Count; i++) {
            result[i] = matches[i].Value;
        }
        
        return result;
    }

    private static bool Eval(string[] templateData, string[] resourceData) {
        if (templateData.Length != resourceData.Length) return false;
        
        for (ushort i = 0; i < templateData.Length; i++) {
            string[] templateStrs = templateData[i].Split('=');
            string[] resourceStrs = resourceData[i].Split('=');
            if (resourceStrs.Length != 2) return false;
            if (templateStrs[0] != resourceStrs[0]) return false;
        }

        return true;
    }
}