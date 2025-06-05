using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalWeapon : WeaponBase
{
    public override void OnHit(CharacterBase_ShadowGrid target)
    {
        target.TakeDamage(baseDamage);
    }
}
