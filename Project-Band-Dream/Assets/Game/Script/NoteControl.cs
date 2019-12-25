using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class NoteControl : MonoBehaviour
{
    // Prefab
    public GameObject notePrefab;
    // Ref
    StateSetting stateSetting = new StateSetting(6f, 1.54f, StateSetting.setBandoriNoteScreenTime(10.5f));
    StageSO stageSO;
    // Note Pool
    List<NoteObject> notePool = new List<NoteObject>();
    // Active Note List
    List<ActiveNote> activeNotes = new List<ActiveNote>();
    // Song time
    public int songTime;
    private void Start()
    {
        // Ref
        stageSO = FindObjectOfType<StageData>().stageSO;
        // Init
        NotePoolInit();
        CreateActiveNote(new MapMetaData.NoteInfo(MapMetaData.NoteInfo.NoteType.Tap, 1, 1000, 1, 0));
        CreateActiveNote(new MapMetaData.NoteInfo(MapMetaData.NoteInfo.NoteType.Hold, 1, 1200, 2, 0));
        CreateActiveNote(new MapMetaData.NoteInfo(MapMetaData.NoteInfo.NoteType.TapOff, 1, 1400, 3, 0));
        CreateActiveNote(new MapMetaData.NoteInfo(MapMetaData.NoteInfo.NoteType.Tick, 1, 1600, 4, 0));
        CreateActiveNote(new MapMetaData.NoteInfo(MapMetaData.NoteInfo.NoteType.Flick, 1, 1800, 5, 0));
        CreateActiveNote(new MapMetaData.NoteInfo(MapMetaData.NoteInfo.NoteType.Tap, 1, 2000, 6, 0));
        CreateActiveNote(new MapMetaData.NoteInfo(MapMetaData.NoteInfo.NoteType.Tap, 1, 2200, 7, 0));
    }

    private void Update()
    {
        // Update Note
        for (int i = 0; i < activeNotes.Count; i++)
        {
            int timeLeft = activeNotes[i].noteInfo.time - songTime;
            if (timeLeft < 0f)
            {
                activeNotes[i].noteObj.gameObj.transform.position = Vector3.up * 100;
                activeNotes.RemoveAt(i);
                i--;
                continue;
            }
            Vector3 v = TimeToPos(timeLeft, activeNotes[i].noteInfo.lane);
            activeNotes[i].noteObj.gameObj.transform.position = new Vector3(v.x, v.y, 0);
            activeNotes[i].noteObj.gameObj.transform.localScale = new Vector3(v.z, v.z, 1);
        }
    }

    // Use note from pool to create ActiveNote
    void CreateActiveNote(MapMetaData.NoteInfo noteInfo)
    {
        NoteObject noteObject = null;
        // Find inactive noteObj
        for (int i = 0; i < notePool.Count; i++)
        {
            if (!notePool[i].active)
            {
                notePool[i].active = true;
                noteObject = notePool[i];
                break;
            }
        }
        // All used? Create more!
        if (noteObject == null)
        {
            noteObject = new NoteObject(Instantiate(notePrefab, Vector3.up * 100, Quaternion.identity), false);
            notePool.Add(noteObject);
        }
        // Set Sprite
        switch (noteInfo.noteType)
        {
            case MapMetaData.NoteInfo.NoteType.Tap:
                noteObject.spriteRenderer.sprite = stageSO.noteTap[noteInfo.lane - 1];
                break;
            case MapMetaData.NoteInfo.NoteType.TapOff:
                noteObject.spriteRenderer.sprite = stageSO.noteTapOff[noteInfo.lane - 1];
                break;
            case MapMetaData.NoteInfo.NoteType.Hold:
                noteObject.spriteRenderer.sprite = stageSO.noteHold[noteInfo.lane - 1];
                break;
            case MapMetaData.NoteInfo.NoteType.Tick:
                noteObject.spriteRenderer.sprite = stageSO.noteTick;
                break;
            case MapMetaData.NoteInfo.NoteType.Flick:
                noteObject.spriteRenderer.sprite = stageSO.noteFlick[noteInfo.lane - 1];
                break;
            default:
                Debug.LogError("WTF Notetype error while creating active note.\nNote:" + noteInfo);
                break;
        }
        // Add to list
        activeNotes.Add(new ActiveNote(noteObject, noteInfo));
    }

    // Create note in pool
    void NotePoolInit()
    {
        for (int i = 0; i < 20; i++)
        {
            notePool.Add(new NoteObject(Instantiate(notePrefab, Vector3.up * 100, Quaternion.identity), false));
        }
    }

    // Note time to position. Stolen shamelessly from Bestdori
    // Return (x,y,scale)
    Vector3 TimeToPos(int time, int lane)
    {
        float a = -0.94f * stateSetting.laneHeight;
        a *= 1 - Mathf.Pow(1.1f, -(time/1000f) / stateSetting.noteScreenTime * 50);
        float s = (a + stateSetting.laneHeight) / stateSetting.laneHeight;
        return new Vector3((lane - 4) * stateSetting.laneWidth * s, -a - 3, s); // Stage offset (y -= 3)
    }

    // Note GameObject
    class NoteObject
    {
        public GameObject gameObj;
        public bool active;
        public SpriteRenderer spriteRenderer;

        public NoteObject(GameObject gameObj, bool active)
        {
            this.gameObj = gameObj;
            this.active = active;
            spriteRenderer = gameObj.GetComponent<SpriteRenderer>();
        }
    }

    // Active Note Data
    class ActiveNote
    {
        public NoteObject noteObj;
        public MapMetaData.NoteInfo noteInfo;

        public ActiveNote(NoteObject noteObj, MapMetaData.NoteInfo noteInfo)
        {
            this.noteObj = noteObj;
            this.noteInfo = noteInfo;
        }
    }
}