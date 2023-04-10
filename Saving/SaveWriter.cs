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
    /* Method for saving the mod data
     * playerName - name of the current player for his saving data */
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

    /* Method for making the backup from file
     * file - the file from which will be made a backup */
    private static void MakeBackup() {
        FileInfo oldFile = SaveFile(".old");
        DateTime timeStamp = oldFile.LastAccessTimeUtc;
        string backupTime = timeStamp.ToString("yyyy-MM-dd-HH-mm-ss-") + timeStamp.Millisecond % 100;
        string postfix = $".backup-{backupTime}";
        oldFile.MoveTo(SaveFile(postfix).FullName);
    }

    /* Method for write the save data into new save file */
    private static void WriteData() {
        /* Create a new save file */
        FileInfo saveFile = SaveFile();
        using FileStream saveStream = saveFile.Create();

        foreach (AchieveJson achieveJson in AchievesContainer.GetAchievementList()) {
            if (AchievesContainer.Has(achieveJson.Id, out Achievement achievement)) {
                byte[] buffer = achievement.SavingData().Crypt();
                saveStream.Write(buffer, 0, buffer.Length);
            } else {
                string savingData = $"{achieveJson.Id}{Repeat(COMPLETED)}{Repeat(ACHIEVE_SEPARATOR)}";
                byte[] buffer = savingData.ToByteArray().Crypt();
                saveStream.Write(buffer, 0, buffer.Length);
            }
        }

        byte[] endOfFile = Repeat(END_FILE).ToByteArray().Crypt();
        saveStream.Write(endOfFile, 0, endOfFile.Length);
    }

    /* Method for removing old backups */
    private static void RemoveBackups() {
        DirectoryInfo saveDir = SaveDirectory();  //Get the save directory
        var backups = saveDir.GetFiles("*.backup-*." + EXTENSION)
                             .OrderBy(e => e.CreationTime.Millisecond).ToArray();  //Get all backups
        
        for (ushort i = 0; i < backups.Length - ConfigValues.NumberOfBackups; i++) 
            backups[i].Delete();  //Delete excess backups
    }
}