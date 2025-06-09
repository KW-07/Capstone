using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : WeaponBase
{
    public float dotDamage = 2f;
    public float dotDuration = 5f;
    public float dotCoolTime = 1f;

    // Inspector 체크박스용
    private void Start()
    {

    }

    public override void OnHit(CharacterBase_ShadowGrid target)
    {
        target.TakeDamage(baseDamage);

        FireEffectHandler fireEffectHandler = target.GetComponent<FireEffectHandler>();
        if(fireEffectHandler != null)
        {
            fireEffectHandler.ApplyDotDamage(dotDamage, dotDuration, dotCoolTime);
        }
    }
}
