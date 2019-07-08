using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public float musicTime;

    private void Update()
    {
        musicTime = audioSource.timeSamples / (float)audioSource.clip.frequency ;
    }
}
