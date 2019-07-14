using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour
{
    const int sourceCount = 10;
    AudioSource[] audioSources = new AudioSource[sourceCount];
    uint channel = 0;

    public enum SoundClip
    {
        HitPerfect,
        HitGreat,
        HitGood,
        HitBad,
        HitMiss,
        HitEmpty
    }
    public AudioClip[] audioClips = new AudioClip[6];

    void Start()
    {
        for (int i = 0; i < sourceCount; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].volume = 0.5f;
        }
    }

    public void PlayOneShot(SoundClip soundClip)
    {
        return; // No sound until delay is fixed

        //audioSources[channel++].PlayOneShot(audioClips[(int)soundClip]);
        //if(channel >= sourceCount)
        //{
        //    channel = 0;
        //}
    }
}
