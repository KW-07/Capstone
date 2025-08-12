using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Buff,
    Debuff
}

public enum BuffTargetStat
{
    Health,
    MoveSpeed,
    Damage,
    Defence
}
public abstract class Buff : ScriptableObject
{
    public string buffName;
    public BuffType type;
    public float duration;
    public BuffTargetStat targetStat;

    public abstract float GetModifierValue();
}
