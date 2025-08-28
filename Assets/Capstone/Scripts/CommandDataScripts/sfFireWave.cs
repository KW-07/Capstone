using UnityEngine;

[CreateAssetMenu(fileName = "sfFireWave", menuName = "CommandData/sfFireWave")]
public class sfFireWave : CommandData
{
    public float projectileSpeed;

    public int hitCount;
    public float hitDamage;
    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        damage = GameObject.Find("Player").GetComponent<PlayerStats>().finalDamage * damage;
        if (effectPrefab != null)
        {
            GameObject projectile = Instantiate(effectPrefab, castPoint.transform.position, castPoint.transform.rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.Initialize(target, damage, projectileSpeed, destroyTime, false);

            var playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

            var hitComponent = projectile.AddComponent<DamageEffect>();
            hitComponent.damage = playerStats.finalDamage * hitDamage;
            hitComponent.hitCount = hitCount;
        }
    }
}