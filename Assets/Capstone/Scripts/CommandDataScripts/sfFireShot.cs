using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "sfFireShot", menuName = "CommandData/sfFireShot")]
public class sfFireShot : CommandData
{
    public float projectileSpeed;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        damage = GameObject.Find("Player").GetComponent<PlayerStats>().finalDamage * damage;
        if (effectPrefab != null)
        {
            GameObject projectile = Instantiate(effectPrefab, castPoint.transform.position, castPoint.transform.rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.Initialize(target, damage, projectileSpeed, destroyTime);
        }
    }
}