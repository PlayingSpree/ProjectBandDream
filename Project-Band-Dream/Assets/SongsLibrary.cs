using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongsLibrary : MonoBehaviour
{
    SongLibraryData songLibrary;
    
    void Start()
    {
        // LoadFromFile
        if (!ExFileManager.LoadFile(ExFileManager.FileType.SongsLibrary,out songLibrary)){
            songLibrary = new SongLibraryData();
            ExFileManager.SaveFile(ExFileManager.FileType.SongsLibrary, songLibrary);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }


    void Update()
    {
        
    }

    [System.Serializable]
    public class SongLibraryData
    {
        public List<SongData> songsList = new List<SongData>();
    }
    [System.Serializable]
    public class SongData
    {
        public string name;
        public string songPath;
    }
}