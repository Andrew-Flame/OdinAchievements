using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AwesomeAchievements.Utility; 

/* Class for work with embedded resources */
internal class ResourceReader {
    private readonly string _resource;
    private readonly Assembly _assembly;

    /* A constructor for creating a new instance of the resource reader using the path of the resource
     * resource - the full path of the resource including its namespace
     * assembly - the assembly where the resource is stored */
    public ResourceReader(string resource, Assembly assembly) {
        _assembly = assembly;
        string assemblyName = assembly.GetName().Name;
        _resource = $"{assemblyName}.{resource}";
    }
    public ResourceReader(string resource) : 
        this(resource, Assembly.GetExecutingAssembly()) { }

    /* Method for getting an enumerator to reading the resource content as a text file line by line
     * returns a string enumerator
     * can throw an exception if it can't read the resource */
    public IEnumerable<string> GetStringReader() {
        using Stream resourceStream = _assembly.GetManifestResourceStream(_resource);
        if (resourceStream == null) throw new UnityException("Can't read embedded resource");
        
        using StreamReader streamReader = new StreamReader(resourceStream);
        if (streamReader == null) throw new UnityException("Can't create a stream reader");
        
        while (!streamReader.EndOfStream) yield return streamReader.ReadLine();
    }

    /* Method for getting a resource content as one string
     * returns a content of resource as a string
     * can throw an exception if it can't read the resource */
    public string ReadAllStrings() {
        using Stream resourceStream = _assembly.GetManifestResourceStream(_resource);
        if (resourceStream == null) throw new UnityException("Can't read embedded resource");
        
        using StreamReader streamReader = new StreamReader(resourceStream);
        if (streamReader == null) throw new UnityException("Can't create a stream reader");
        
        return streamReader.ReadToEnd();
    }

    /* Method for getting a resource content as a byte array
     * returns a content of resource as a byte array
     * can throw an exception if it can't read the resource */
    public byte[] ReadAllBytes() {
        using Stream resourceStream = _assembly.GetManifestResourceStream(_resource);
        if (resourceStream == null) throw new UnityException("Can't read embedded resource");
        
        using BinaryReader binaryReader = new BinaryReader(resourceStream!);
        if (binaryReader == null) throw new UnityException("Can't create a binary reader");
        
        return binaryReader.ReadBytes((int)resourceStream.Length);
    }

    /* Method for writing the resource data into the file
     * filePath - the path of the file for writing
     * can throw an exception if it can't read the resource or if it can't create a file */
    public void WriteToFile(string filePath) {
        if (File.Exists(filePath)) return;
        
        using Stream resourceStream = _assembly.GetManifestResourceStream(_resource);
        if (resourceStream == null) throw new UnityException("Can't read embedded resource");

        using FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        if (fileStream == null) throw new UnityException("Can't create a file");
        
        resourceStream.CopyTo(fileStream);
    }

    /* Method for writing the resource data into temp file
     * fileName - the name of the temp file
     * returns the full path for the temp file */
    public string WriteToTmp(string fileName) {
        string filePath = $@"{Path.GetTempPath()}/{fileName}";
        WriteToFile(filePath);
        return filePath;
    }

    /* Method fot getting the sprite from the resource
     * return the sprite */
    public Sprite GetSprite() {
        Texture2D panelTexture = new Texture2D(0, 0);  //Init random texture (it will be resized)
        panelTexture.LoadImage(ReadAllBytes(), false);  //Load an image from resource
        return Sprite.Create(panelTexture, new Rect(0f, 0f, panelTexture.width, panelTexture.height), new Vector2(0f, 0f));  //Create a new sprite
    }
}