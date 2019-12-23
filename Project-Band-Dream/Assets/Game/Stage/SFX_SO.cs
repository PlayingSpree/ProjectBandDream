using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFX_SO", menuName = "ScriptableObject/SFX_SO", order = 1)]
public class SFX_SO : ScriptableObject
{
    public AudioClip tap;
    public AudioClip flick;
}