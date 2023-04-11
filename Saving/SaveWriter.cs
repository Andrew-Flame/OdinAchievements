using System;
using System.IO;
using System.Linq;
using AwesomeAchievements.AchieveLists;
using AwesomeAchievements.Achieves;
using AwesomeAchievements.Utility;
using static AwesomeAchievements.Saving.SaveManager;

namespace AwesomeAchievements.Saving; 

/* Class for writing save files */
internal static class SaveWriter {
    /* Method for saving the mod data */
    public static void Save() {
        RenameFiles();  //Rename old files
        WriteData();  //Write save data
        RemoveBackups();  //Remove excess backups
        LogInfo.Log($"The {ModInfo.TITLE} data was successfully saved");
    }

    /* Method for rename old save files */
    private static void RenameFiles() {
        FileInfo saveFile = SaveFile(),
                 oldFile = SaveFile(".old");

        /* If .old file exists */
        if (oldFile.Exists) 
            MakeBackup();  //Make a backup using it
        
        /* If the main save file exists */
        if (saveFile.Exists) 
            saveFile.MoveTo(SaveFile(".old").FullName);  //Make the .old file using it
    }

    /* Method for making the backup from file */
    private static void MakeBackup() {
        FileInfo oldFile = SaveFile(".old");  //Get the old file save path
        DateTime timeStamp = oldFile.LastAccessTimeUtc;  //Get the time stamp
        string backupTime = timeStamp.ToString("yyyy-MM-dd-HH-mm-ss-") + timeStamp.Millisecond % 100;  //Get the formatted backup time
        string postfix = $".backup-{backupTime}";  //Get the postfix for the new file name
        oldFile.MoveTo(SaveFile(postfix).FullName);  //Move the old file to the new backup file
    }

    /* Method for write the save data into new save file */
    private static void WriteData() {
        /* Create a new save file */
        FileInfo saveFile = SaveFile();
        using FileStream saveStream = saveFile.Create();

        foreach (AchieveJson achieveJson in AchievesContainer.GetAchievementList()) {
            if (AchievesContainer.Has(achieveJson.Id, out Achievement achievement)) {  //If the achievement is not completed
                byte[] buffer = achievement.SavingData();  //Get the saving data from the achievement
                saveStream.Write(buffer.Encrypt(), 0, buffer.Length);  //Write the encrypted data to the file
            } else {  //If the achievement is already completed
                string savingData = $"{achieveJson.Id}{Repeat(COMPLETED)}{Repeat(ACHIEVE_SEPARATOR)}";  //Create a saving data
                byte[] buffer = savingData.ToByteArray();  //Create the buffer from the saving data
                saveStream.Write(buffer.Encrypt(), 0, buffer.Length);  //Write the encrypted data to the file
            }
        }

        /* Write the end of file to the file */
        byte[] endOfFile = Repeat(END_FILE).ToByteArray();
        saveStream.Write(endOfFile.Encrypt(), 0, endOfFile.Length);
    }

    /* Method for removing old backups */
    private static void RemoveBackups() {
        DirectoryInfo saveDir = SaveDirectory();  //Get the save directory
        var backups = saveDir.GetFiles("*.backup-*." + EXTENSION).OrderBy(e => e.CreationTime.Millisecond).ToArray();  //Get all backups
        for (ushort i = 0; i < backups.Length - ConfigValues.NumberOfBackups; i++) backups[i].Delete();  //Delete excess backups
    }
}