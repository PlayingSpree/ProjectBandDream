using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ExFileManager
{
    public enum FileType
    {
        SongsLibrary
    }
    // File Path
    static string filePath = Application.persistentDataPath + @"\";

    // File Name
    static string GetFileName(FileType fileType)
    {
        switch (fileType)
        {
            case FileType.SongsLibrary:
                return "SongsLibrary.bin";
            default:
                Debug.LogError("Invalid File Type :" + fileType);
                return null;
        }
    }
    public static bool SaveFile(FileType fileType, object obj)
    {
        BinaryFormatter BF = new BinaryFormatter();
        FileStream F;
        try
        {
            F = File.Create(filePath + GetFileName(fileType));
            BF.Serialize(F, obj);
        }
        catch (System.Exception s)
        {
            Debug.LogError(s);
            return false;
        }
        if (F != null)
        {
            F.Close();
        }
        return true;
    }
    public static bool LoadFile<T>(FileType fileType, out T obj) where T : class
    {
        if (File.Exists(filePath + GetFileName(fileType)))
        {
            BinaryFormatter BF = new BinaryFormatter();
            FileStream F;
            try
            {
                F = File.Open(filePath + GetFileName(fileType), FileMode.Open);
                obj = (T)System.Convert.ChangeType(BF.Deserialize(F), typeof(T));
            }
            catch (System.Exception s)
            {
                Debug.LogError(s);
                obj = null;
                return false;
            }
            if (F != null)
            {
                F.Close();
            }
        }
        else
        {
            obj = null;
            return false;
        }
        return true;
    }

}
