using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_Chase", menuName = "Player/Commands/F_Chase")]
public class F_ChaseCommandData : CommandData
{
    public float projectileSpeed;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName} น฿ป็!");

        if (effectPrefab != null)
        {
            GameObject projectile = Instantiate(effectPrefab, castPoint.transform.position, castPoint.transform.rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.Initialize(target, damage, projectileSpeed, destroyTime);
        }
    }
}
