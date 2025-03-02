using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Player/Commands/Melee")]
public class MeleeAttackData : CommandData
{
    public float attackRange;
    public int multipleAttack = 0;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName}(MeleeAttack) 사용");
        switch(multipleAttack)
        {
            // 기본공격 1타
            case 1:
                if (effectPrefab != null)
                {
                    GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
                    Debug.Log(multipleAttack);
                    Destroy(effect, destroyTime);
                }
                break;
            // 기본공격 2타
            case 2:
                break;
            // 기본공격 3타
            case 3:
                break;
            default:
                break;
        }

        // 생성위치 충돌 체크 및 적이면 데미지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPoint.transform.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"{enemy.name}에게 {damage}의 피해를 입힘!");
        }
    }
}
