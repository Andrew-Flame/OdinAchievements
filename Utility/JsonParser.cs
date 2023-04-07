using System.Collections.Generic;
using System.Text.RegularExpressions;
using AwesomeAchievements.AchieveLists;

namespace AwesomeAchievements.Utility; 

/* Class for work with json achievement lists */
internal sealed class JsonParser {
    private readonly string _data;

    public int AchievesCount => Regex.Matches(_data, "id").Count;

    public JsonParser(string data) => _data = data;

    public IEnumerable<AchieveJson> ParseAchieves() {
        Regex substringRegex = new Regex(@"{\s*(""(id|name|description)""\s*:\s*"".*""\s*,?\s*){3}}");
        MatchCollection matches = substringRegex.Matches(_data);

        foreach (Match match in matches) {
            string jsonObjStr = match.Value;

            string id = GetValue("id"),
                   name = GetValue("name"),
                   description = GetValue("description");

            string GetValue(string key) {
                string line = Regex.Match(jsonObjStr, @$"""{key}""\s*:\s*"".*""").Value;
                return line.Split(':')[1].Trim().Replace("\"", "").Trim();
            }
            
            yield return new AchieveJson(id, name, description);
        }
    }
}