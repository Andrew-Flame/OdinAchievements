namespace AwesomeAchievements.Saving; 

internal readonly struct SavingData {
    public const char ACHIEVE_SEPARATOR = (char)0,
                      ENUM_BEGINNER = (char)1,
                      ENUM_SEPARATOR = (char)2,
                      END_FILE_CHAR = (char)3;

    public const byte OFFSET = 179;
}