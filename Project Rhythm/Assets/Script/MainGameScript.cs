using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameplayData;

public class MainGameScript : MonoBehaviour
{
    public GameMusic gameMusic;
    public GameSound gameSound;

    public GameObject[] notePrefab = new GameObject[3];
    public GameObject[] noteSlidePrefab = new GameObject[2];
    public GameObject hitBox;

    Vector2[] spawnPoint;
    GameObject[] hitPoint;
    ParticleSystem[] hitPaticle;

    GameData.StageSetting stageSetting;
    GameData.MapData songData;
    GameData.GameplaySetting gameplaySetting;

    [SerializeField]
    float musicSpeed = 1f;
    float noteSpeed = 0.5f;

    [SerializeField]
    float musicTime = -3f;
    [SerializeField]
    float musicSyncTime = 0f;
    float lastTime = 0f;

    uint noteCount = 0u;
    List<Note> noteObject = new List<Note>(30);
    List<Note> noteDelete = new List<Note>(30);

    void Awake()
    {
        stageSetting = new GameData.StageSetting();
        gameplaySetting = new GameData.GameplaySetting();
        spawnPoint = new Vector2[stageSetting.lane];
        hitPoint = new GameObject[stageSetting.lane];
        hitPaticle = new ParticleSystem[stageSetting.lane];
        songData = GameData.Data.LoadMapData(69);
    }

    void Start()
    {
        gameMusic.audioSource.PlayScheduled(AudioSettings.dspTime - musicTime);
        SetStage();
    }

    void Update()
    {
        #region MusicAndTimingControl

        gameMusic.audioSource.pitch = musicSpeed;
        musicTime += Time.deltaTime * musicSpeed;
        if (gameMusic.audioSource.time > 0f)
        {
            musicSyncTime += Time.deltaTime * musicSpeed;
            if (lastTime != gameMusic.audioSource.time)
            {
                lastTime = gameMusic.audioSource.time;
                musicSyncTime = lastTime + gameplaySetting.delay;
            }
            musicTime = Mathf.Lerp(musicTime, musicSyncTime, Time.deltaTime * 15f);
        }
        else
        {
            gameMusic.audioSource.SetScheduledStartTime(AudioSettings.dspTime - musicTime + gameplaySetting.delay);
        }

        #endregion

        #region NoteSpawn
        if (noteCount < songData.noteCount)
        {
            while (songData.notes[noteCount].time < musicTime + noteSpeed)
            {
                noteObject.Add(new Note(noteCount, Instantiate(notePrefab[(int)songData.notes[noteCount].type], (Vector3)spawnPoint[songData.notes[noteCount].lane], Quaternion.identity, transform), songData.notes[noteCount]));
                noteCount++;
                if (noteCount == songData.noteCount)
                {
                    break;
                }
            }
        }
        #endregion

        #region NoteUpdate

        foreach (Note item in noteObject)
        {
            float t = (item.noteData.time - musicTime) / (musicSpeed*noteSpeed);
            //All Perfect Hit (Temp)
            if((musicSyncTime >= item.noteData.time))
            {
                hitPaticle[item.noteData.lane].Play();
                gameSound.PlayOneShot(GameSound.SoundClip.HitPerfect);
                Destroy(item.noteObject);
                noteDelete.Add(item);
                continue;
            }
            if (musicSyncTime - item.noteData.time > gameplaySetting.noteTiming[3])
            {
                Destroy(item.noteObject);
                noteDelete.Add(item);
                continue;
            }
            item.noteObject.transform.position = Vector3.LerpUnclamped(hitPoint[item.noteData.lane].transform.position, spawnPoint[item.noteData.lane], t) + Vector3.forward * (noteCount / 100f);
            item.noteObject.transform.localScale = Vector3.one * Mathf.LerpUnclamped(stageSetting.noteBehavior.endSize, stageSetting.noteBehavior.startSize, t);
        }
        //Need Optimize???
        foreach (Note item in noteDelete)
        {
            noteObject.Remove(item);
        }
        noteDelete.Clear();

        #endregion
    }

    void SetStage()
    {
        for (int i = 0; i < stageSetting.lane; i++)
        {
            spawnPoint[i] = stageSetting.spawnPoint[i];
            hitPoint[i] = Instantiate(hitBox, stageSetting.hitPoint[i], Quaternion.identity, transform);
            hitPoint[i].transform.localScale = stageSetting.hitPointSize[i];
            hitPaticle[i] = hitPoint[i].GetComponentInChildren<ParticleSystem>();
        }
    }

    //For Debugger

    public GameData.MapData GetSongData()
    {
        return songData;
    }

    public float GetMusicTime()
    {
        return musicSyncTime;
    }

    public GameData.GameplaySetting GetGameplaySetting()
    {
        return gameplaySetting;
    }
}