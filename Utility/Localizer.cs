// ReSharper disable UnusedAutoPropertyAccessor.Local
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AwesomeAchievements.Utility; 

/* Class for get all localizable texts for this mod */
internal static class Localizer {
    /* Properties for getting localized texts */
    public static string AchievePanelHeader { get; private set; }
    public static string ChatMessage { get; private set; }
    
    /* Method for initializing this type
     * can throw an exception if this mod doesn't support a language from the config file */
    public static void Init() {
        if (!AvailableLangs().Contains(ConfigValues.Language))  //If there isn't such a language in the resources, throw an exception
            throw new UnityException($"This modification does not support such a language ({ConfigValues.Language}). Please, change it in the configuration");
        string localePath = $"Locales.{ConfigValues.Language}.ini";  //Get a path of the locale resource
        ResourceReader localeReader = new ResourceReader(localePath);  //Create an resource reader for locale file
        var properties = typeof(Localizer).GetProperties();  //Get an array of properties in this class
        
        foreach (string locale in localeReader.GetStringReader()) {  //Cycle through the string of the locale file
            foreach (PropertyInfo property in properties) {  //Cycle through the array of properties
                if (!locale.StartsWith(property.Name)) continue;  //If a string of the locale file starts with the name of property
                string[] partedLocale = locale.Trim().Split('=');  //Split the key and the value of the string of the locale file
                string localeValue = partedLocale[partedLocale.Length - 1].Trim();  //Get value of the string of the locale file
                property.SetValue(null, localeValue);  //Set this value to the static property
            }
        }

        LogInfo.Log("A locale container has been initialized");
    }

    /* Method fot getting a list of available languages */
    public static IEnumerable<string> AvailableLangs() {
        Assembly assembly = Assembly.GetExecutingAssembly();
        const string localesNamespace = "AwesomeAchievements.Locales.";

        foreach (var locale in assembly.GetManifestResourceNames())
            if (locale.StartsWith(localesNamespace) && !locale.Contains("template"))
                yield return locale.Substring(localesNamespace.Length, 3);
    }
}