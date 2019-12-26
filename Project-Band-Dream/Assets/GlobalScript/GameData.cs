using System.Collections.Generic;

namespace GameData
{
    [System.Serializable]
    public static class GameSetting
    {
        public static float musicVolume = 0.3f;
        public static float noteSpeed = 1.0f;
        public static StateSetting stateSetting = new StateSetting(6f, 1.54f, StateSetting.setBandoriNoteScreenTime(noteSpeed));
        public static NoteHitTiming noteHitTiming = new NoteHitTiming();
    }

    [System.Serializable]
    public class MapData
    {
        public string name;
        public string songPath;
        public int previewPoint = 0;
        public List<NoteInfo> notes = new List<NoteInfo>();

        public MapData(string name, string songPath)
        {
            this.name = name;
            this.songPath = songPath;
        }

        public struct NoteInfo
        {
            public enum NoteType
            {
                Tap, TapOff, Hold, Tick, Flick
            }
            public NoteType noteType;

            public int time;
            public int lane;
            public int next;

            public NoteInfo(NoteType noteType, int time, int lane, int next)
            {
                this.noteType = noteType;
                this.time = time;
                this.lane = lane;
                this.next = next;
            }
        }
    }

    public class StateSetting
    {
        public float laneHeight;
        public float laneWidth;
        public float noteScreenTime;
        public float laneHitExtraSize = 1.5f;

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

    public struct NoteHitTiming
    {
        public int perfect;
        public int great;
        public int good;
        public int bad;

        public NoteHitTiming(int perfect = 50, int great = 100, int good = 117, int bad = 133)
        {
            this.perfect = perfect;
            this.great = great;
            this.good = good;
            this.bad = bad;
        }
    }
}