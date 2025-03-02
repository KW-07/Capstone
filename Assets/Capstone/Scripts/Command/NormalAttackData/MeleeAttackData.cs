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
        Debug.Log($"{commandName}(MeleeAttack) ���");
        switch(multipleAttack)
        {
            // �⺻���� 1Ÿ
            case 1:
                if (effectPrefab != null)
                {
                    GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
                    Debug.Log(multipleAttack);
                    Destroy(effect, destroyTime);
                }
                break;
            // �⺻���� 2Ÿ
            case 2:
                break;
            // �⺻���� 3Ÿ
            case 3:
                break;
            default:
                break;
        }

        // ������ġ �浹 üũ �� ���̸� ������
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPoint.transform.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"{enemy.name}���� {damage}�� ���ظ� ����!");
        }
    }
}
