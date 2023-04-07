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
        Regex substringRegex = new Regex(@"{(""(id|name|description)"":""[^""]+"",?){3}}");
        MatchCollection matches = substringRegex.Matches(_data);

        foreach (Match match in matches) {
            string jsonObjStr = match.Value;

            string id = GetValue("id"),
                   name = GetValue("name"),
                   description = GetValue("description");

            /* Method for getting a value of achievement json object by it's key */
            string GetValue(string key) =>
                Regex.Match(jsonObjStr, @$"""{key}"":""[^""]*""").Value
                     .Split(':')[1].Trim(' ', '\"');

            yield return new AchieveJson(id, name, description);
        }
    }
}