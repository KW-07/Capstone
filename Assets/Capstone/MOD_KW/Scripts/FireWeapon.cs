using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : WeaponBase
{
    public float dotDamage = 2f;
    public float dotDuration = 5f;
    public float dotCoolTime = 1f;

    public override void OnHit(CharacterBase_ShadowGrid target)
    {
        target.TakeDamage(baseDamage);
        target.StartCoroutine(ApplyDot(target));
    }

    private IEnumerator ApplyDot(CharacterBase_ShadowGrid target)
    {
        float elapsed = 0f;

        while (elapsed < dotDuration)
        {
            target.TakeDamage(dotDamage);
            yield return new WaitForSeconds(dotCoolTime);
            elapsed += dotCoolTime;
        }
    }
}
