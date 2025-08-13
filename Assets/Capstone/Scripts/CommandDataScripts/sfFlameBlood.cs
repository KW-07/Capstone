using UnityEngine;

[CreateAssetMenu(fileName = "sfFlameBlood", menuName = "CommandData/sfFlameBlood")]
public class sfFlameBlood : CommandData
{
    public GameObject buffEffect;
    public Buff chriticalChanceBuff;

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
        if (buffManager != null && chriticalChanceBuff != null)
        {
            buffManager.AddBuff(chriticalChanceBuff);
        }
        else
        {
            Debug.LogWarning("BuffManager / attackPowerBuff 없음");
        }
    }
}