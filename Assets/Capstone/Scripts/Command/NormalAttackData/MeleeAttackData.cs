using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Player/Commands/Melee")]
public class MeleeAttackData : CommandData
{
    public float attackRange;
    public int multipleAttack = 0;
    public int firstCountDamage;
    public int secondCountDamage;
    public int thirdCountDamage;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName}(MeleeAttack) ���");
        switch(multipleAttack)
        {
            // �⺻���� 1Ÿ
            case 1:
                damage = firstCountDamage;
                if (effectPrefab != null)
                {
                    GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
                    Debug.Log("firstCount");
                    Destroy(effect, destroyTime);
                }
                break;
            // �⺻���� 2Ÿ
            case 2:
                damage = secondCountDamage;
                if (effectPrefab != null)
                {
                    GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
                    Debug.Log("secondCount");
                    Destroy(effect, destroyTime);
                }
                break;
            // �⺻���� 3Ÿ
            case 3:
                damage = thirdCountDamage;
                if (effectPrefab != null)
                {
                    GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
                    Debug.Log("thirdCount");
                    Destroy(effect, destroyTime);
                }
                break;
            default:
                break;
        }

        // ������ġ �浹 üũ �� ���̸� ������
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPoint.transform.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"{enemy.name}���� {PlayerAttack.instance.playerDamage + damage}�� ���ظ� ����!");
        }
    }
}
