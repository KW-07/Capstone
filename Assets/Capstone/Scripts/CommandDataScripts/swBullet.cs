using UnityEngine;

[CreateAssetMenu(fileName = "swBullet", menuName = "CommandData/swBullet")]
public class swBullet : CommandData
{
    public int initCount;
    public float projectileSpeed;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        damage = GameObject.Find("Player").GetComponent<PlayerStats>().finalDamage * damage; // 데미지 초기화

        if (effectPrefab != null)
        {
            for (int i = 0; i < initCount; i++) // 원거리 이펙트 개수만큼 생성 및 발사
            {
                GameObject projectile = Instantiate(effectPrefab, castPoint.transform.position, castPoint.transform.rotation);   // 투사체 생성
                Projectile projectileScript = projectile.GetComponent<Projectile>(); // 투사체 스크립트 참조
                projectileScript.Initialize(target, damage, projectileSpeed, destroyTime, false); // 투사체 스크립트에 필요한 정보(타겟, 데미지, 투사체속도, 유지시간, 유도 T/F) 부여, 유도할 것이면 T, 그냥 투사체면 F

                var playerStats = GameObject.Find("Player").GetComponent<PlayerStats>(); // 플레이어 스탯

                var hitComponent = projectile.AddComponent<DamageEffect>();
                hitComponent.damage = playerStats.finalDamage * damage;
            }
        }

    }
}