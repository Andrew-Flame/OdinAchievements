// ReSharper disable UnusedAutoPropertyAccessor.Local
using System.Reflection;

namespace AwesomeAchievements.Utility; 

/* Class for get all localizable texts for this mod */
internal static class Localizer {
    /* Properties for getting localized texts */
    public static string AchievePanelHeader { get; private set; }
    public static string ChatMessage { get; private set; }
    
    /* Method for initializing this type */
    public static void Init() {
        const string resourceNamespace = "AwesomeAchievements.Localization";  //The namespace with locale files
        ResourceReader localeReader = new ResourceReader($"{resourceNamespace}.{ConfigValues.Language}.ini");  //Create an resource reader for locale file
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
}