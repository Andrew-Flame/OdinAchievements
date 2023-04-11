using System.IO;
using System.Text;
using AwesomeAchievements.GameClasses;
using AwesomeAchievements.Utility;

namespace AwesomeAchievements.Saving; 

internal static class SaveManager {
    public const char COMPLETED = (char)0,
                      ACHIEVE_SEPARATOR = (char)29,
                      ENUM_BEGINNER = (char)30,
                      ENUM_SEPARATOR = (char)31,
                      END_FILE = (char)3;
    public const string EXTENSION = "ach";
    private const byte OFFSET = 208;
    private const byte REPEAT = 3;

    /* Method for getting the save directory
     * returns the directory info of the save directory */
    public static DirectoryInfo SaveDirectory() {
        if (Directory.Exists(ConfigValues.SavePath))  //If there is a custom save path
            return new DirectoryInfo(ConfigValues.SavePath);  //Get a custom save path
        else 
            return GamePlayerProfile.GetSaveDirectory();  //Else get the save directory where saves are stored
    }

    /* Method for getting the save file info
     * returns the info of the save file */
    public static FileInfo SaveFile() => SaveFile(string.Empty);
    public static FileInfo SaveFile(string postfix) {
        string playerName = GamePlayerProfile.GetPlayerName();  //Get player name
        return new FileInfo($"{SaveDirectory().FullName}/{playerName.ToLower()}{postfix}.{EXTENSION}");  //Get the save file using the save dir and player name
    }

    /* Method for getting the repeating char as a string
     * chr - the char for repeating */
    public static string Repeat(char chr) => new (chr, REPEAT);

    /* Method for converting the string to the byte array
     * str - the string for converting */
    public static byte[] ToByteArray(this string str) => Encoding.ASCII.GetBytes(str);
    
    /* Method for encrypting the byte array
     * buffer - the byte array for encrypting */
    public static byte[] Encrypt(this byte[] buffer) {
        for (int i = 0; i < buffer.Length; i++)
            unchecked { buffer[i] += OFFSET; }
        return buffer;
    }

    /* Method for decrypting the byte array
     * buffer - the byte array for decrypting */
    public static byte[] Decrypt(this byte[] buffer) {
        for (int i = 0; i < buffer.Length; i++)
            unchecked { buffer[i] -= OFFSET; }
        return buffer;
    }
}