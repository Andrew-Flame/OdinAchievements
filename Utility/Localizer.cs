using System.Reflection;

namespace AwesomeAchievements.Utility; 

internal static class Localizer {
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    public static string AchievePanelHeader { get; private set; }
    
    public static void Init(string language) {
        const string resourceNamespace = "AwesomeAchievements.Localization";
        ResourceReader localeReader = new ResourceReader($"{resourceNamespace}.{language}.ini");
        var properties = typeof(Localizer).GetProperties();
        
        foreach (string locale in localeReader.GetStringReader()) {
            foreach (PropertyInfo property in properties) {
                if (!locale.StartsWith(property.Name)) continue;
                string[] partedLocale = locale.Trim().Split('=');
                string localeValue = partedLocale[partedLocale.Length - 1].Trim();
                property.SetValue(null, localeValue);
            }
        }
    }
}