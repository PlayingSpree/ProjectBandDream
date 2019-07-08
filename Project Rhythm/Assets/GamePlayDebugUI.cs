using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayDebugUI : MonoBehaviour
{
    public Text beatCount;
    public Text TxDelay;

    public MainGameScript gameScript;
    public GameData.MapData songData;
    public GameData.GameplaySetting gameplaySetting;

    uint currentTiming = 0;

    private void Start()
    {
        songData = gameScript.GetSongData();
        gameplaySetting = gameScript.GetGameplaySetting();

        TxDelay.text = "Delay : " + gameplaySetting.delay;
    }

    void Update()
    {
        if (currentTiming < songData.timingCount - 1)
        {
            if (songData.timings[currentTiming + 1].time > gameScript.GetMusicTime())
            {
                currentTiming++;
            }
        }
        int currentBeat = Mathf.FloorToInt((gameScript.GetMusicTime() - songData.timings[currentTiming].time) * (songData.timings[currentTiming].bpm*(songData.timings[currentTiming].bpb / 60f)));
        beatCount.text = string.Format("{1}:{0}.{2}", (currentBeat/ songData.timings[currentTiming].bpb) % songData.timings[currentTiming].bpb, currentBeat / (songData.timings[currentTiming].bpb* songData.timings[currentTiming].bpb),currentBeat % songData.timings[currentTiming].bpb);
    }

    public void AddDelay(float delay)
    {
        gameplaySetting.delay += delay;
        TxDelay.text = string.Format("Delay : {0:f4}",gameplaySetting.delay);
    }
}