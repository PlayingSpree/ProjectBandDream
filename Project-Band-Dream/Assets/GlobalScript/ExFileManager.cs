using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public static class ExFileManager
{
    public enum FileType
    {
        MapsLibrary
    }
    // File Path
    static readonly string filePath = Application.persistentDataPath + @"\";
    // File Name
    static string GetFileName(FileType fileType)
    {
        switch (fileType)
        {
            case FileType.MapsLibrary:
                return "SongsLibrary.bddata";
            default:
                Debug.LogError("Invalid File Type :" + fileType);
                return null;
        }
    }
    // Save/Load File
    public static bool SaveFilePath(string path, object obj)
    {
        try
        {
            string d = Path.GetDirectoryName(path);
            if (!Directory.Exists(d))
            {
                Directory.CreateDirectory(d);
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(JsonUtility.ToJson(obj));
            }
        }
        catch (System.Exception s)
        {
            Debug.LogError(s);
            return false;
        }

        return true;
    }
    public static bool LoadFilePath(string path, object obj)
    {
        if (File.Exists(path))
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    JsonUtility.FromJsonOverwrite(sr.ReadToEnd(), obj);
                }
            }
            catch (System.Exception s)
            {
                Debug.LogError(s);
                obj = null;
                return false;
            }
        }
        else
        {
            obj = null;
            return false;
        }
        return true;
    }
    public static bool SaveFile(FileType fileType, object obj)
    {
        return SaveFilePath(Path.Combine(filePath, GetFileName(fileType)), obj);
    }
    public static bool LoadFile(FileType fileType, object obj)
    {
        return LoadFilePath(filePath + GetFileName(fileType), obj);
    }

    public static IEnumerator LoadMp3(string path, System.Action<AudioClip> func)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                func(DownloadHandlerAudioClip.GetContent(www));
            }
        }
    }
}
