using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource[] audioSources = new AudioSource[2];
    int activeAudioSource = 0;
    float fadeout = 0f;
    float fadein = 0f;

    // Temp
    float musicVolume = 1f;
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
                audioSources[1 - activeAudioSource].volume = fadeout * musicVolume;
                fadeout -= Time.deltaTime;
            }
            else
            {
                audioSources[1 - activeAudioSource].Stop();
            }
        }
        if (fadein > 0f)
        {
            audioSources[activeAudioSource].volume = (1f - fadein) * musicVolume;
            fadein -= Time.deltaTime;
        }
        else
        {
            audioSources[activeAudioSource].volume = musicVolume;
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

    public void Play(AudioClip clip, int ms = 0, double delay = 1d)
    {
        Stop();
        activeAudioSource = 1 - activeAudioSource;
        if (audioSources[activeAudioSource].isPlaying)
        {
            audioSources[activeAudioSource].Stop();
        }
        audioSources[activeAudioSource].clip = clip;
        audioSources[activeAudioSource].PlayScheduled(AudioSettings.dspTime + delay);
        SetSongTime(ms);
        fadein = 0.5f + (float)delay;
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
