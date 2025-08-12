using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PosionBuff", menuName = "Buffs/PoisonBuff")]
public class PosionBuff : Buff
{
    //public override void OnApply(CharacterStats target)
    //{
    //    foreach (var effect in effects)
    //    {
    //        effect.OnApply(target);
    //    }
    //}

    //public override void OnUpdate(CharacterStats target, float deltaTime)
    //{
    //    foreach (var effect in effects)
    //    {
    //        effect.OnUpdate(target, deltaTime);
    //    }
    //}

    //public override void OnRemove(CharacterStats target)
    //{
    //    foreach (var effect in effects)
    //    {
    //        effect.OnRemove(target);
    //    }
    //}
    public override float GetModifierValue()
    {
        return 1;
    }
}
