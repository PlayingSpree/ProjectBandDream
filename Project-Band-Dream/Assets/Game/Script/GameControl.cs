using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    MusicPlayer musicPlayer;
    NoteControl noteControl;
    double songPlayTime;
    double lastDspTime;
    int lastSongTime;

    void Start()
    {
        // Ref
        musicPlayer = FindObjectOfType<MusicPlayer>();
        noteControl = FindObjectOfType<NoteControl>();
        musicPlayer.Play(SongSelect.audioClip, 0, 3d);
        songPlayTime = AudioSettings.dspTime + 3d;
    }

    void Update()
    {
        UpdateSongTime();
    }

    private void UpdateSongTime()
    {
        if (AudioSettings.dspTime < songPlayTime)
        {
            noteControl.songTime = (int)((AudioSettings.dspTime - songPlayTime) * 1000d);
        }
        else
        {
            int songTime = musicPlayer.GetSongTime();
            if (songTime == lastSongTime)
            {
                noteControl.songTime += (int)((AudioSettings.dspTime - lastDspTime) * 1000d);
            }
            else
            {
                noteControl.songTime = songTime;
            }

            lastDspTime = AudioSettings.dspTime;
            lastSongTime = songTime;
        }
    }
}
