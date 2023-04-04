using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AwesomeAchievements.Utility; 

/* A class for working with embedded resources */
internal class ResourceReader {
    private readonly Assembly _assembly;
    private readonly string _resource;

    /* A constructor for creating a new instance of the resource reader using the path of the resource
     * resource - the full path of the resource including its namespace */
    public ResourceReader(string resource) {
        _assembly = Assembly.GetExecutingAssembly();
        _resource = resource;
    }

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
}