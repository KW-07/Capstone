using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_Fierce", menuName = "Player/Commands/F_Fierce")]
public class F_FierceCommandData : CommandData
{
    public float duration;
    public float speedMultiplier;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName} (버프) 사용! 속도 {speedMultiplier}배 증가, 지속시간 {duration}초");

        Player movement = castPoint.GetComponent<Player>();
        if (movement != null)
        {
            movement.ApplySpeedBuff(speedMultiplier, duration);
        }

        // 버프 오브젝트 즉시 삭제
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
            effect.transform.parent = castPoint.transform;

            Destroy(effect, duration); // 효과 지속 시간 후 자동 삭제
        }
    }
}
