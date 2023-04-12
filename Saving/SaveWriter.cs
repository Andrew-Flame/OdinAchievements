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
            oldFile.MakeBackup();  //Make a backup using it
        
        /* If the main save file exists */
        if (saveFile.Exists) 
            saveFile.MoveTo(SaveFile(".old").FullName);  //Make the .old file using it
    }

    /* Method for making the backup from file
     * oldFile - the old file for creating a backup of it */
    private static void MakeBackup(this FileInfo oldFile) {
        DateTime timeStamp = oldFile.LastWriteTime;  //Get the time stamp
        string date = timeStamp.ToString("yyyy-MM-dd-HH-mm-ss-");  //Get the date of the last file write
        string milliseconds = (timeStamp.Millisecond % 100).ToString();  //Get milliseconds of the last file write
        string backupTime = date + (milliseconds.Length < 2 ? '0' + milliseconds : milliseconds);  //Get the formatted backup time
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
                string savingData = $"{achieveJson.Id}{COMPLETED.Repeat()}{ACHIEVE_SEPARATOR.Repeat()}";  //Create a saving data
                byte[] buffer = savingData.ToByteArray();  //Create the buffer from the saving data
                saveStream.Write(buffer.Encrypt(), 0, buffer.Length);  //Write the encrypted data to the file
            }
        }

        /* Write the end of file to the file */
        byte[] endOfFile = END_FILE.Repeat().ToByteArray();
        saveStream.Write(endOfFile.Encrypt(), 0, endOfFile.Length);
    }

    /* Method for removing old backups */
    private static void RemoveBackups() {
        DirectoryInfo saveDir = SaveDirectory();  //Get the save directory
        var backups = saveDir.GetFiles("*.backup-*." + EXTENSION).OrderBy(e => e.CreationTime.Millisecond).ToArray();  //Get all backups
        for (ushort i = 0; i < backups.Length - ConfigValues.NumberOfBackups; i++) backups[i].Delete();  //Delete excess backups
    }
    
    /* Method for encrypting the byte array
     * buffer - the byte array for encrypting */
    private static byte[] Encrypt(this byte[] buffer) {
        for (int i = 0; i < buffer.Length; i++)
            unchecked { buffer[i] += OFFSET; }
        return buffer;
    }
}