using UnityEngine;

[CreateAssetMenu(fileName = "swUntie", menuName = "CommandData/swUntie")]
public class swUntie : CommandData
{
    public Buff evadeChanceBuff;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        if (effectPrefab != null)
        {
            Debug.Log($"{commandNameKor} 사용"); // 해당 스킬 잘 사용하는지 디버그 확인

            GameObject effectInstance = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity); // 오브젝트 생성

            var follow = effectInstance.AddComponent<FollowCaster>(); // 플레이어 따라다닐 이펙트 생성
            follow.caster = castPoint; // 이펙트 위치값 플레이어값으로 초기화

            Destroy(effectInstance, destroyTime); // 초기이펙트 오브젝트 삭제
        }

        BuffManager buffManager = castPoint.GetComponent<BuffManager>(); // 버프매니저 스크립트
        if (buffManager != null && evadeChanceBuff != null)
        {
            buffManager.AddBuff(evadeChanceBuff); // 버프매니저에 해당 스킬의 버프 등록
        }

    }
}