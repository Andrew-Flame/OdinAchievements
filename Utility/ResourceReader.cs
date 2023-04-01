using System.IO;
using System.Reflection;
using UnityEngine;

namespace AwesomeAchievements.Utility; 

internal class ResourceReader {
    private readonly Assembly _assembly;
    private readonly string _resource;

    public ResourceReader(string resource) {
        _assembly = Assembly.GetExecutingAssembly();
        _resource = resource;
    }
    
    public string ReadString() {
        using Stream resourceStream = _assembly.GetManifestResourceStream(_resource);
        if (resourceStream == null) throw new UnityException("Can't read embedded resource");
        
        using StreamReader streamReader = new StreamReader(resourceStream!);
        if (streamReader == null) throw new UnityException("Can't create a stream reader");

        return streamReader.ReadToEnd();
    }

    public void Save(string filePath) {
        if (File.Exists(filePath)) return;  //If the file exists, return false
        using Stream resourceStream = _assembly.GetManifestResourceStream(_resource);  //Create a resource stream
        using FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);  //Create a file stream

        /* If streams are null, throw new exceptions */
        if (resourceStream == null) throw new UnityException("Can't read embedded resource");
        if (fileStream == null) throw new UnityException("Can't write data to file");
        resourceStream.CopyTo(fileStream);  //Copy bytes from one stream to the other
    }

    public string SaveTmp(string fileName) {
        string tempPath = Path.GetTempPath();  //Get the temp path
        string filePath = $@"{tempPath}\{fileName}";  //Get a new file path
        Save(filePath);
        return filePath;
    }

    public byte[] ReadAllBytes() {
        using Stream resourceStream = _assembly.GetManifestResourceStream(_resource);  //Create a resource stream
        if (resourceStream == null) throw new UnityException("Can't read embedded resource");
        
        using BinaryReader binaryReader = new BinaryReader(resourceStream);  //Create a binary reader
        if (binaryReader == null) throw new UnityException("Can't create a binary reader");

        return binaryReader.ReadBytes((int)resourceStream.Length);
    }
}