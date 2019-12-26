using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class NoteControl : MonoBehaviour
{
    // Prefab
    public GameObject notePrefab;
    // Ref
    StageSO stageSO;

    // Data
    MapData map = SongCreator.CreateSong();
    List<NoteObject> notePool = new List<NoteObject>();
    List<ActiveNote> activeNotes = new List<ActiveNote>();

    HitDetection hitDetection = new HitDetection();
    public bool autoMode = false;

    public int songTime;
    float scaledNoteScreenTime;

    public int noteCount;
    public List<int> spawnedNote = new List<int>();
    private void Start()
    {
        // Ref
        stageSO = FindObjectOfType<StageData>().stageSO;
        // Init
        NotePoolInit();
    }

    private void Update()
    {
        // Create Note
        if (noteCount < map.notes.Count)
        {
            while (map.notes[noteCount].time < songTime + (int)(scaledNoteScreenTime * 1000f))
            {
                // Spawn? (By next)
                if (spawnedNote.Contains(noteCount))
                {
                    spawnedNote.Remove(noteCount);
                }
                else
                {
                    CreateActiveNote(map.notes[noteCount]);
                }
                // Next Note
                int n = map.notes[noteCount].next;
                if (n > 0)
                {
                    CreateActiveNote(map.notes[n]);
                    spawnedNote.Add(n);
                }
                noteCount++;
                if (noteCount == map.notes.Count)
                {
                    break;
                }
            }
        }
        // Detect Hit
        hitDetection.Update();
        // Update Note
        for (int i = 0; i < activeNotes.Count; i++)
        {
            int timeLeft = activeNotes[i].noteInfo.time - songTime;
            if (timeLeft < 0f)
            {
                RemoveActiveNote(i);
                i--;
                continue;
            }
            Vector3 v = TimeToPos(timeLeft, activeNotes[i].noteInfo.lane);
            activeNotes[i].noteObj.gameObj.transform.position = new Vector3(v.x, v.y, 0);
            activeNotes[i].noteObj.gameObj.transform.localScale = new Vector3(v.z, v.z, 1);
        }
    }

    void RemoveActiveNote(int i)
    {
        activeNotes[i].noteObj.gameObj.transform.position = Vector3.up * 100;
        activeNotes[i].noteObj.active = false;
        activeNotes.RemoveAt(i);
    }

    // Use note from pool to create ActiveNote
    void CreateActiveNote(MapData.NoteInfo noteInfo)
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
            noteObject.active = true;
            notePool.Add(noteObject);
        }
        // Set Sprite
        switch (noteInfo.noteType)
        {
            case MapData.NoteInfo.NoteType.Tap:
                noteObject.spriteRenderer.sprite = stageSO.noteTap[noteInfo.lane];
                break;
            case MapData.NoteInfo.NoteType.TapOff:
                noteObject.spriteRenderer.sprite = stageSO.noteTapOff[noteInfo.lane];
                break;
            case MapData.NoteInfo.NoteType.Hold:
                noteObject.spriteRenderer.sprite = stageSO.noteHold[noteInfo.lane];
                break;
            case MapData.NoteInfo.NoteType.Tick:
                noteObject.spriteRenderer.sprite = stageSO.noteTick;
                break;
            case MapData.NoteInfo.NoteType.Flick:
                noteObject.spriteRenderer.sprite = stageSO.noteFlick[noteInfo.lane];
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
        float a = -0.94f * GameSetting.stateSetting.laneHeight;
        a *= 1 - Mathf.Pow(1.1f, -(time / 1000f) / scaledNoteScreenTime * 50f);
        float s = (a + GameSetting.stateSetting.laneHeight) / GameSetting.stateSetting.laneHeight;
        return new Vector3((lane - 3) * GameSetting.stateSetting.laneWidth * s, -a - 3, s); // Stage offset (y -= 3)
    }

    public void SetSpeed(float songSpeed)
    {
        scaledNoteScreenTime = Mathf.Max(0.05f, GameSetting.stateSetting.noteScreenTime * songSpeed);
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
        public MapData.NoteInfo noteInfo;

        public ActiveNote(NoteObject noteObj, MapData.NoteInfo noteInfo)
        {
            this.noteObj = noteObj;
            this.noteInfo = noteInfo;
        }
    }
}