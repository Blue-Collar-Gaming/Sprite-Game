using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stats
{
    #region Stat Variables
    public float maxHitPoints = 10;
    public float hitPoints = 10;
    public float defense = 1;
    #endregion

    public Stats(float hp, float def)
    {
        hitPoints = hp;
        defense = def;
    }
}
