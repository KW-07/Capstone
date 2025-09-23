using UnityEngine;

[CreateAssetMenu(fileName = "sfFlameLance", menuName = "CommandData/sfFlameLance")]
public class sfFlameLance : CommandData
{
    public float projectileSpeed;

    public int hitCount;

    public int initCount;
    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        damage = GameObject.Find("Player").GetComponent<PlayerStats>().finalDamage * damage;
        if (effectPrefab != null)
        {
            for(int i = 0;i < initCount; i++)
            {
                GameObject projectile = Instantiate(effectPrefab, castPoint.transform.position, castPoint.transform.rotation);
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.Initialize(target, damage, projectileSpeed, destroyTime, true); // À¯µµ

                var playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

                var hitComponent = projectile.AddComponent<DamageEffect>();
                hitComponent.damage = playerStats.finalDamage * damage;
                hitComponent.hitCount = hitCount;
            }
        }
    }
}