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
        var resourceStream = OpenResourceStream();
        var streamReader = OpenStreamReader(resourceStream);
        while (!streamReader.EndOfStream) yield return streamReader.ReadLine();
    }

    public string ReadAllStrings() {
        var resourceStream = OpenResourceStream();
        var streamReader = OpenStreamReader(resourceStream);
        return streamReader.ReadToEnd();
    }

    public byte[] ReadAllBytes() {
        var resourceStream = OpenResourceStream();
        var binaryReader = OpenBinaryReader(resourceStream);
        return binaryReader.ReadBytes((int)resourceStream.Length);
    }

    private Stream OpenResourceStream() {
        using Stream resourceStream = _assembly.GetManifestResourceStream(_resource);
        if (resourceStream == null) throw new UnityException("Can't read embedded resource");
        return resourceStream;
    }

    private static StreamReader OpenStreamReader(Stream resourceStream) {
        using StreamReader streamReader = new StreamReader(resourceStream!);
        if (streamReader == null) throw new UnityException("Can't create a stream reader");
        return streamReader;
    }

    private static BinaryReader OpenBinaryReader(Stream resourceStream) {
        using BinaryReader binaryReader = new BinaryReader(resourceStream!);
        if (binaryReader == null) throw new UnityException("Can't create a binary reader");
        return binaryReader;
    }
}