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

    // 스탯 관련 전용
    public BuffTargetStat targetStat;
    
    public List<BuffEffect> effects;

    public abstract void Apply(CharacterStats target);
    public abstract void Remove(CharacterStats target);
}
