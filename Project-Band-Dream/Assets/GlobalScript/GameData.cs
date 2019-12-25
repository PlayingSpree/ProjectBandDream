using System.Collections.Generic;

namespace GameData
{
    [System.Serializable]
    public static class GameSetting
    {
        public static float musicVolume = 0.3f;
        public static float noteSpeed = 1.0f;
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
}