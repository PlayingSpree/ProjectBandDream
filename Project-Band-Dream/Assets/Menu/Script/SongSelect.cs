using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SongSelect : MonoBehaviour
{
    MusicPlayer musicPlayer;
    int selectedSong;

    void Start()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        MapsLibrary.Scan();
        MapsLibrary.Save();
    }

    void Update()
    {

    }

    public void SelectSong(int index)
    {
        selectedSong = index;
        StartCoroutine(ExFileManager.LoadMp3(Path.Combine(MapsLibrary.data.songsList[index].mapPath, MapsLibrary.data.songsList[index].songPath), LoadFinish));
    }

    void LoadFinish(AudioClip clip)
    {
        musicPlayer.Play(clip, MapsLibrary.data.songsList[selectedSong].previewPoint);
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
