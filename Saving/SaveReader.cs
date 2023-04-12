using static AwesomeAchievements.Saving.SaveManager;

namespace AwesomeAchievements.Saving;

/* Class for reading save files */
internal static class SaveReader {
    /* Method for decrypting the byte array
     * buffer - the byte array for decrypting */
    public static byte[] Decrypt(this byte[] buffer) {
        for (int i = 0; i < buffer.Length; i++)
            unchecked { buffer[i] -= OFFSET; }
        return buffer;
    }
}