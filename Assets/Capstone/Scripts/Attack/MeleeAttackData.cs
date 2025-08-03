using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Player/Attack/Melee")]
public class MeleeAttackData : CommandData
{
    float attackRange;
    public int multipleAttack = 0;
    public int firstCountDamage;
    public int secondCountDamage;
    public int thirdCountDamage;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        //Debug.Log($"{commandName}(MeleeAttack) 사용");
        switch(multipleAttack)
        {
            // 기본공격 1타
            case 1:
                damage = firstCountDamage;
                if (effectPrefab != null)
                {
                    GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
                    Destroy(effect, destroyTime);
                }
                break;
            // 기본공격 2타
            case 2:
                damage = secondCountDamage;
                if (effectPrefab != null)
                {
                    GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
                    Destroy(effect, destroyTime);
                }
                break;
            // 기본공격 3타
            case 3:
                damage = thirdCountDamage;
                if (effectPrefab != null)
                {
                    GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
                    Destroy(effect, destroyTime);
                }
                break;
            default:
                break;
        }

        // 생성위치 충돌 체크 및 적이면 데미지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPoint.transform.position, effectPrefab.GetComponent<Transform>().localScale.x, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"{enemy.name}에게 {Player.instance.playerDamage + damage}의 피해를 입힘!");
            enemy.GetComponent<LivingEntity>().OnDamage(Player.instance.playerDamage + damage);
        }
    }
}
