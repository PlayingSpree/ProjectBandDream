using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage_SO", menuName = "ScriptableObject/Stage_SO", order = 1)]
public class StageSO : ScriptableObject
{
    public Sprite[] noteTap;
    public Sprite[] noteTapOff;
    public Sprite[] noteFlick;
    public Sprite noteFlickIcon;
    public Sprite[] noteHold;
    public Sprite noteTick;
    public Sprite bg;
    public Sprite lane;
    public Sprite laneHit;
}