using System.Collections.Generic;
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

    public IEnumerable<string> GetStringReader() {
        using Stream resourceStream = _assembly.GetManifestResourceStream(_resource);
        if (resourceStream == null) throw new UnityException("Can't read embedded resource");
        
        using StreamReader streamReader = new StreamReader(resourceStream);
        if (streamReader == null) throw new UnityException("Can't create a stream reader");
        
        while (!streamReader.EndOfStream) yield return streamReader.ReadLine();
    }

    public string ReadAllStrings() {
        using Stream resourceStream = _assembly.GetManifestResourceStream(_resource);
        if (resourceStream == null) throw new UnityException("Can't read embedded resource");
        
        using StreamReader streamReader = new StreamReader(resourceStream);
        if (streamReader == null) throw new UnityException("Can't create a stream reader");
        
        return streamReader.ReadToEnd();
    }

    public byte[] ReadAllBytes() {
        using Stream resourceStream = _assembly.GetManifestResourceStream(_resource);
        if (resourceStream == null) throw new UnityException("Can't read embedded resource");
        
        using BinaryReader binaryReader = new BinaryReader(resourceStream!);
        if (binaryReader == null) throw new UnityException("Can't create a binary reader");
        
        return binaryReader.ReadBytes((int)resourceStream.Length);
    }
}