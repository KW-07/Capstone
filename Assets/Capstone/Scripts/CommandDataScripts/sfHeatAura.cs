using UnityEngine;

[CreateAssetMenu(fileName = "sfHeatAura", menuName = "CommandData/sfHeatAura")]
public class sfHeatAura : CommandData
{
    public GameObject buffEffect;
    public Buff lifeDrainBuff;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandNameKor} 스킬 사용");

        if (castPoint == null)
        {
            Debug.LogWarning("CastPoint가 없습니다.");
            return;
        }

        if (effectPrefab != null)
        {
            GameObject effectInstance = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);

            var follow = effectInstance.AddComponent<FollowCaster>();
            follow.caster = castPoint;

            Destroy(effectInstance, destroyTime);
        }

        BuffManager buffManager = castPoint.GetComponent<BuffManager>();
        if (buffManager != null && lifeDrainBuff != null)
        {
            buffManager.AddBuff(lifeDrainBuff);
        }
        else
        {
            Debug.LogWarning("BuffManager / attackPowerBuff 없음");
        }
    }
}