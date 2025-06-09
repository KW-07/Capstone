using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class NormalWeapon : WeaponBase
{
    // Inspector 체크박스용
    private void Start()
    {
        
    }
    public override void OnHit(CharacterBase_ShadowGrid target)
    {
        target.TakeDamage(baseDamage);

        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0f;//일직선으로만

        KnockbackHandler knockbackHandler = target.GetComponent<KnockbackHandler>();
        if (knockbackHandler != null)
        {
            knockbackHandler.ApplyKnockback(direction, knockbackForce, knockbackDuration);
        }
    }
}
