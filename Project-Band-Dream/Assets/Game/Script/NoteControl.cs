using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // TEMP!!! Song time
    float songTime = 0f;
    private void Start()
    {
        // Late Ref
        stageSO = FindObjectOfType<StageData>().stageSO;
        // Init
        notePoolInit();
        createActiveNote(new NoteInfo(NoteInfo.NoteType.Tap, 1, 1f, 1, 0));
        createActiveNote(new NoteInfo(NoteInfo.NoteType.Hold, 1, 1.2f, 2, 0));
        createActiveNote(new NoteInfo(NoteInfo.NoteType.TapOff, 1, 1.4f, 3, 0));
        createActiveNote(new NoteInfo(NoteInfo.NoteType.Tick, 1, 1.6f, 4, 0));
        createActiveNote(new NoteInfo(NoteInfo.NoteType.Flick, 1, 1.8f, 5, 0));
        createActiveNote(new NoteInfo(NoteInfo.NoteType.Tap, 1, 2f, 6, 0));
        createActiveNote(new NoteInfo(NoteInfo.NoteType.Tap, 1, 2.2f, 7, 0));
    }


    private void Update()
    {
        songTime += Time.deltaTime;
        // Update Note
        for (int i = 0; i < activeNotes.Count; i++)
        {
            float timeLeft = activeNotes[i].noteInfo.time - songTime;
            if (timeLeft < 0f)
            {
                activeNotes[i].noteObj.gameObj.transform.position = Vector3.up * 100;
                activeNotes.RemoveAt(i);
                i--;
                continue;
            }
            Vector3 v = timeToPos(timeLeft, activeNotes[i].noteInfo.lane);
            activeNotes[i].noteObj.gameObj.transform.position = new Vector3(v.x, v.y, 0);
            activeNotes[i].noteObj.gameObj.transform.localScale = new Vector3(v.z, v.z, 1);
        }
    }

    // Use note from pool to create ActiveNote
    void createActiveNote(NoteInfo noteInfo)
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
            case NoteInfo.NoteType.Tap:
                noteObject.spriteRenderer.sprite = stageSO.noteTap[noteInfo.lane - 1];
                break;
            case NoteInfo.NoteType.TapOff:
                noteObject.spriteRenderer.sprite = stageSO.noteTapOff[noteInfo.lane - 1];
                break;
            case NoteInfo.NoteType.Hold:
                noteObject.spriteRenderer.sprite = stageSO.noteHold[noteInfo.lane - 1];
                break;
            case NoteInfo.NoteType.Tick:
                noteObject.spriteRenderer.sprite = stageSO.noteTick;
                break;
            case NoteInfo.NoteType.Flick:
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
    void notePoolInit()
    {
        for (int i = 0; i < 20; i++)
        {
            notePool.Add(new NoteObject(Instantiate(notePrefab, Vector3.up * 100, Quaternion.identity), false));
        }
    }

    // Note time to position. Stolen shamelessly from Bestdori
    // Return (x,y,scale)
    Vector3 timeToPos(float time, int lane)
    {
        float a = -0.94f * stateSetting.laneHeight;
        a *= 1 - Mathf.Pow(1.1f, -time / stateSetting.noteScreenTime * 50);
        float s = (a + stateSetting.laneHeight) / stateSetting.laneHeight;
        return new Vector3((lane - 4) * stateSetting.laneWidth * s, -a - 3, s); // Stage offset (y -= 3)
    }

    // Active Note
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

    // Active Note
    class ActiveNote
    {
        public NoteObject noteObj;
        public NoteInfo noteInfo;

        public ActiveNote(NoteObject noteObj, NoteInfo noteInfo)
        {
            this.noteObj = noteObj;
            this.noteInfo = noteInfo;
        }
    }
}

// Temp class for Game Setting
public class StateSetting
{
    public float laneHeight;
    public float laneWidth;
    public float noteScreenTime;

    public StateSetting(float laneHeight, float laneWidth, float noteScreenTime)
    {
        this.laneHeight = laneHeight;
        this.laneWidth = laneWidth;
        this.noteScreenTime = noteScreenTime;
    }

    // Note screen time. Also Stolen shamelessly from Bestdori
    public static float setBandoriNoteScreenTime(float simNoteSpeed)
    {
        return 5.5f - (simNoteSpeed - 1) / 2;
    }
}

// Temp!!! Note information
struct NoteInfo
{
    public enum NoteType
    {
        Tap, TapOff, Hold, Tick, Flick
    }
    public NoteType noteType;

    public int id;
    public float time;
    public int lane;
    public int next;

    public NoteInfo(NoteType noteType, int id, float time, int lane, int next)
    {
        this.noteType = noteType;
        this.id = id;
        this.time = time;
        this.lane = lane;
        this.next = next;
    }
}