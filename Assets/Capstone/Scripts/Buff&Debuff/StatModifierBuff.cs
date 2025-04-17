using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatModifierBuff", menuName = "Buffs/StatModifierBuff")]

public class StatModifierBuff : Buff
{
    public float value;

    public override void Apply(CharacterStats target)
    {
        target.ModifyStat(targetStat, value);
    }

    public override void Remove(CharacterStats target)
    {
        target.ModifyStat(targetStat, -value);
    }
}
