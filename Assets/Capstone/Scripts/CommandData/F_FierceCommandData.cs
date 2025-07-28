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
        Debug.Log($"{commandName} (����) ���! �ӵ� {speedMultiplier}�� ����, ���ӽð� {duration}��");

        Player movement = castPoint.GetComponent<Player>();
        if (movement != null)
        {
            movement.ApplySpeedBuff(speedMultiplier, duration);
        }

        // ���� ������Ʈ ��� ����
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
            effect.transform.parent = castPoint.transform;

            Destroy(effect, duration); // ȿ�� ���� �ð� �� �ڵ� ����
        }
    }
}
