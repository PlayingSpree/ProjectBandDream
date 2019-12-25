using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class MapsLibrary
{
    public static MapsLibraryData data = new MapsLibraryData();

    public static void Load()
    {
        // LoadFromFile
        if (!ExFileManager.LoadFile(ExFileManager.FileType.MapsLibrary, data))
        {
            data = new MapsLibraryData();
            ExFileManager.SaveFile(ExFileManager.FileType.MapsLibrary, data);
        }
    }

    public static void Save()
    {
        ExFileManager.SaveFile(ExFileManager.FileType.MapsLibrary, data);
    }

    // Scan for map
    public static void Scan()
    {
        try
        {
            var files = Directory.EnumerateFiles(Application.persistentDataPath, "*.bdmap", SearchOption.AllDirectories);
            foreach (string currentFile in files)
            {
                string path = Path.GetDirectoryName(currentFile);
                MapMetaData m = new MapMetaData("-", "-");
                if (ExFileManager.LoadFilePath(currentFile, m))
                {
                    // Add map
                    data.songsList.Add(new MapEntryData(path, currentFile, m.name, m.songPath , m.previewPoint));
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    [System.Serializable]
    public class MapsLibraryData
    {
        public List<MapEntryData> songsList = new List<MapEntryData>();
    }
    [System.Serializable]
    public class MapEntryData
    {
        public string mapPath;
        public string mapMetaFile;
        public string name;
        public string songPath;
        public int previewPoint;

        public MapEntryData(string mapPath, string mapMetaFile, string name, string songPath, int previewPoint)
        {
            this.mapPath = mapPath;
            this.mapMetaFile = mapMetaFile;
            this.name = name;
            this.songPath = songPath;
            this.previewPoint = previewPoint;
        }
    }
}