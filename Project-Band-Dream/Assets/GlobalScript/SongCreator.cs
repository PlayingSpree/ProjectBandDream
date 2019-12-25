using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongCreator
{
    public static MapData CreateSong()
    {
        MapData m = new MapData("-", "-");
        int x = 0;
        for (int i = 0; i < 1800; i++)
        {
            m.notes.Add(new MapData.NoteInfo(MapData.NoteInfo.NoteType.Tap, (int)((((i + x) * (60f / 520f)) + 5.242f) * 1000f), (((i + x) % 2) * 6) + 1, -1));
            if (i % 3 == 2)
            {
                x++;
            }
        }
        return m;
    }
}
