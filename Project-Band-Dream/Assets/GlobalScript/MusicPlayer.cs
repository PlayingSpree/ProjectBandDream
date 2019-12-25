using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource[] audioSources = new AudioSource[2];
    int activeAudioSource = 0;
    float fadeout = 0f;
    float fadein = 0f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < 2; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].bypassEffects = true;
        }
    }

    void Update()
    {
        // Fade Effect
        if (audioSources[1 - activeAudioSource].isPlaying)
        {
            if (fadeout > 0f)
            {
                audioSources[1 - activeAudioSource].volume = fadeout * GameData.GameSetting.musicVolume;
                fadeout -= Time.deltaTime;
            }
            else
            {
                audioSources[1 - activeAudioSource].Stop();
            }
        }
        if (fadein > 0f)
        {
            audioSources[activeAudioSource].volume = (1f - fadein) * GameData.GameSetting.musicVolume;
            fadein -= Time.deltaTime;
        }
        else
        {
            audioSources[activeAudioSource].volume = GameData.GameSetting.musicVolume;
        }
    }

    public int GetSongTime()
    {
        return audioSources[activeAudioSource].timeSamples / (audioSources[activeAudioSource].clip.frequency / 1000);
    }

    public void SetSongTime(int ms)
    {
        audioSources[activeAudioSource].timeSamples = ms * (audioSources[activeAudioSource].clip.frequency / 1000);
    }

    public float GetSongSpeed()
    {
        return audioSources[activeAudioSource].pitch;
    }

    public void SetSongSpeed(float speed)
    {
        audioSources[activeAudioSource].pitch = speed;
    }

    public void Play(AudioClip clip, int time = 0, double delay = 1d, bool nofade = false, float speed = 1f)
    {
        Stop();
        activeAudioSource = 1 - activeAudioSource;
        if (audioSources[activeAudioSource].isPlaying)
        {
            audioSources[activeAudioSource].Stop();
        }
        audioSources[activeAudioSource].clip = clip;
        audioSources[activeAudioSource].PlayScheduled(AudioSettings.dspTime + delay);
        SetSongTime(time);
        SetSongSpeed(speed);
        fadein = nofade ? 0f : (0.5f + (float)delay);
    }

    public void Stop()
    {
        if (audioSources[activeAudioSource].isPlaying)
        {
            if (fadeout <= 0f)
            {
                fadeout = 1f;
            }
        }
    }
}
