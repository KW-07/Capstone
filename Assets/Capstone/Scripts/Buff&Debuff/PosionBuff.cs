using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PosionBuff", menuName = "Buffs/PoisonBuff")]
public class PosionBuff : Buff
{
    public override void Apply(CharacterStats target)
    {
        foreach (var effect in effects)
        {
            effect.OnApply(target);
        }
    }

    public override void Remove(CharacterStats target)
    {
        foreach (var effect in effects)
        {
            effect.OnRemove(target);
        }
    }
}
