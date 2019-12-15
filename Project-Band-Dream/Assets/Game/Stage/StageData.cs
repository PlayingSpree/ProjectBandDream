using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    public StageSO stageSO;

}

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

[CreateAssetMenu(fileName = "SFX_SO", menuName = "ScriptableObject/SFX_SO", order = 1)]
public class SFX_SO : ScriptableObject
{
    public AudioClip tap;
    public AudioClip flick;
}