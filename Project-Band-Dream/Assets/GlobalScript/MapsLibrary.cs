using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GameData;

public static class MapsLibrary
{
    public static MapsLibraryData data = new MapsLibraryData();

    public static bool Load()
    {
        // LoadFromFile
        if (!ExFileManager.LoadFile(ExFileManager.FileType.MapsLibrary, data))
        {
            data = new MapsLibraryData();
            ExFileManager.SaveFile(ExFileManager.FileType.MapsLibrary, data);
            return false;
        }
        return true;
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
                MapData m = new MapData("-", "-");
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
        public string mapFile;
        public string name;
        public string songPath;
        public int previewPoint;

        public MapEntryData(string mapPath, string mapFile, string name, string songPath, int previewPoint)
        {
            this.mapPath = mapPath;
            this.mapFile = mapFile;
            this.name = name;
            this.songPath = songPath;
            this.previewPoint = previewPoint;
        }
    }
}