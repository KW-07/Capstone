using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeAttack", menuName = "Player/Attack/Range")]
public class RangeAttackData : CommandData
{
    public float projectileSpeed;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName} น฿ป็!");

        if(effectPrefab != null)
        {
            GameObject projectile = Instantiate(effectPrefab, castPoint.transform.position, castPoint.transform.rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.Initialize(target, damage, projectileSpeed, destroyTime);

        }
    }
}
