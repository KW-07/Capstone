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
            // ����
            Quaternion rot = Player.instance.facingRight ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, rot);

            // ����
            var hitComponent = effect.AddComponent<DamageEffect>();
            hitComponent.damage = damage;
            hitComponent.hitCount = hitCount;

            // ȸ�ǹ���
            BuffManager buffManager = castPoint.GetComponent<BuffManager>();
            if (buffManager != null && evadeBuff != null)
            {
                buffManager.AddBuff(evadeBuff);
            }

            Destroy(effect, destroyTime);
        }
    }
}