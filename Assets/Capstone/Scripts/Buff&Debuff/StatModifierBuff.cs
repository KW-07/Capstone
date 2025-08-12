using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatModifierBuff", menuName = "Buffs/StatModifierBuff")]

public class StatModifierBuff : Buff
{
    public float damageIncreaseAmount;

    public override float GetModifierValue()
    {
        return (GameObject.Find("Player").GetComponent<PlayerStats>().finalDamage * damageIncreaseAmount);
    }
}
