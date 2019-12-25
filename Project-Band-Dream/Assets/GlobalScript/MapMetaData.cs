[System.Serializable]
public class MapMetaData
{
    public string name;
    public string songPath;
    public int previewPoint = 0;

    public MapMetaData(string name, string songPath)
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

        public int id;
        public int time;
        public int lane;
        public int next;

        public NoteInfo(NoteType noteType, int id, int time, int lane, int next)
        {
            this.noteType = noteType;
            this.id = id;
            this.time = time;
            this.lane = lane;
            this.next = next;
        }
    }
}