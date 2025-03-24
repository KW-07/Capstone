using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : LivingEntity
{
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        Debug.Log("Player took damage! Current Health: " + currentHealth);
    }

    public override void OnDie()
    {
        base.OnDie();
        Debug.Log("Player is dead.");
    }
}
