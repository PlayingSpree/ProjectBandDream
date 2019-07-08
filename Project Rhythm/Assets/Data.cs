using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData{
    public static class Data
    {
        public static MapData LoadMapData(int mapID)
        {
            //temp
            MapData map = new MapData
            {
                noteCount = 1800,
                notes = new Note[1800],
                noteSlideCount = 0,
                slideNotes = new NoteSlide[] { },
                timingCount = 1,
                timings = new Timing[] { new Timing(5.242f,130f,4) }
            };
            int x = 0;
            for (int i = 0; i < 1800; i++)
            {
                map.notes[i] = new Note(Note.NoteType.Tap,((i+x)%2)*6,((i+x)*(60f/520f))+5.242f);
                if (i % 3 == 2)
                {
                    x++;
                }
            }
            return map;
        }
    }
    public class MapData
    {
        public uint noteCount;
        public Note[] notes;
        public uint noteSlideCount;
        public NoteSlide[] slideNotes;
        public uint timingCount;
        public Timing[] timings;
    }
    public class Note
    {
        public enum NoteType {Tap,Flick,Slide};
        public NoteType type;
        public int lane;
        public float time;

        public Note(NoteType type, int lane, float time)
        {
            this.type = type;
            this.lane = lane;
            this.time = time;
        }
    }
    public class NoteSlide
    {
        public int tick;
        public int[] lane;
        public float[] time;
    }
    public class Timing
    {
        public float time;
        public float bpm;
        public int bpb;

        public Timing(float time, float bpm, int bpb)
        {
            this.time = time;
            this.bpm = bpm;
            this.bpb = bpb;
        }
    }
    public class StageSetting
    {
        public NoteBehavior noteBehavior;
        public uint lane;
        public Vector2[] spawnPoint;
        public Vector2[] hitPoint;
        public Vector2[] hitPointSize;

        public StageSetting()
        {
            StageBandori();
        }

        public void StageBandori()
        {
            noteBehavior = new NoteBehavior();
            lane = 7;
            spawnPoint = new Vector2[] { new Vector2(-3, 4), new Vector2(-2, 4), new Vector2(-1, 4), new Vector2(0, 4), new Vector2(1, 4), new Vector2(2, 4), new Vector2(3, 4) };
            hitPoint = new Vector2[] { new Vector2(-6, -4), new Vector2(-4, -4), new Vector2(-2, -4), new Vector2(0, -4), new Vector2(2, -4), new Vector2(4, -4), new Vector2(6, -4) };
            hitPointSize = new Vector2[] { new Vector2(4, 4), new Vector2(4, 4), new Vector2(4, 4), new Vector2(4, 4), new Vector2(4, 4), new Vector2(4, 4), new Vector2(4, 4) };
        }
    }
    public class GameplaySetting
    {
        public float[] noteTiming = new float[]{0.05f, 0.1f, 0.11667f, 0.13333f};
        public float delay = 0.0f;
    }
    public class NoteBehavior
    {
        public float startSize = 0.5f;
        public float endSize = 1f;
        public float fadeIn = 0.05f;
    }
}
namespace GameplayData
{
    public class Note
    {
        public GameObject noteObject;
        public uint note;
        public GameData.Note noteData;

        public Note(uint note, GameObject noteObject , GameData.Note noteData)
        {
            this.note = note;
            this.noteObject = noteObject;
            this.noteData = noteData;
        }
    }
}