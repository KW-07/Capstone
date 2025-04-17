using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PosionEffect", menuName = "Effects/PoisonEffect")]
public class PoisonEffect : BuffEffect
{
    public float damagePerSecond;

    public override void OnApply(CharacterStats target) { }

    public override void OnUpdate(CharacterStats target, float deltaTime)
    {
        //target.TakeDamage(damagePerSecond * deltaTime);
    }

    public override void OnRemove(CharacterStats target) { }
}
