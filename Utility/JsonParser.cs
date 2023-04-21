using System.Collections.Generic;
using System.Text.RegularExpressions;
using VikingAchievements.AchieveLists;
using UnityEngine;

namespace VikingAchievements.Utility; 

/* Class for work with json achievement lists */
internal sealed class JsonParser {
    private readonly string _data;

    public int AchievesCount => Regex.Matches(_data, "\"id\"").Count;

    /* Constructor for creating new parser instance
     * data - json string
     * can throw an exception if the json string as too short */
    public JsonParser(string data) {
        Regex jsonStringRegex = new Regex(@"{\s*""data"":\s*\[(\s*{(\s*(""(id|name|description)""\s*:\s*"".*"",?\s*)*)},?)*\s*\]\s*}");
        //Regex for getting the json string from the input string
        Regex compressRegex = new Regex(@"(?<=[\s"":\{\}\[\],])\s(?=[\s"":\{\}\[\],])");
        //Regex for compress the regex string

        /* Get clear json string */
        _data = jsonStringRegex.Match(data).Value;
        _data = compressRegex.Replace(_data, string.Empty);

        /* Throw an exception if the json string is too short */
        if (_data.Length < 5) throw new UnityException("Can't parse a json string");
    }

    /* Method for getting the sequence if achievement json objects
     * returns the IEnumerable of achievement json objects */
    public IEnumerable<AchieveJson> ParseAchieves() {
        /* Get achievement json matches form json string */
        Regex substringRegex = new Regex(@"{(""(id|name|description)"":""[^""]*"",?){3}}");
        MatchCollection matches = substringRegex.Matches(_data);

        foreach (Match match in matches) {
            string jsonObjStr = match.Value;  //Cycle through matches

            string id = GetValue("id"),  //Get id value
                   name = GetValue("name"),  //Get name value
                   description = GetValue("description");  //Get description value

            /* Method for getting a value of achievement json object by it's key */
            string GetValue(string key) => Regex.Match(jsonObjStr, @$"""{key}"":""[^""]*""").Value.Split(':')[1].Trim(' ', '\"');

            yield return new AchieveJson(id, name, description);  //Yield return the achievement json object
        }
    }
}