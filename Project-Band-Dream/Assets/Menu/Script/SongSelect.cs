using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SongSelect : MonoBehaviour
{
    MusicPlayer musicPlayer;
    public static int selectedSong;
    public static AudioClip audioClip;

    void Start()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        if (!MapsLibrary.Load())
        {
            MapsLibrary.Scan();
            MapsLibrary.Save();
        }
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
        audioClip = clip;
        musicPlayer.Play(clip, MapsLibrary.data.songsList[selectedSong].previewPoint);
    }

    public void Play()
    {
        FindObjectOfType<SceneController>().ToGame();
    }
}
