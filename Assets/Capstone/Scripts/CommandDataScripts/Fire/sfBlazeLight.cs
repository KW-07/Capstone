using UnityEngine;

[CreateAssetMenu(fileName = "sfBlazeLight", menuName = "CommandData/sfBlazeLight")]
public class sfBlazeLight : CommandData
{
    public int hitCount;

    public Buff evadeBuff;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        if (effectPrefab != null)
        {
            // 생성
            Quaternion rot = Player.instance.facingRight ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, rot);

            // 공격
            var hitComponent = effect.AddComponent<DamageEffect>();
            hitComponent.damage = damage;
            hitComponent.hitCount = hitCount;

            // 회피버프
            BuffManager buffManager = castPoint.GetComponent<BuffManager>();
            if (buffManager != null && evadeBuff != null)
            {
                buffManager.AddBuff(evadeBuff);
            }

            Destroy(effect, destroyTime);
        }
    }
}