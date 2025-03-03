using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_Fist", menuName = "Player/Commands/F_Fist")]
public class F_FistCommandData : CommandData
{
    public float attackRange;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName}(F_Fist) ���");

        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
            Destroy(effect, destroyTime);
        }

        // ������ġ �浹 üũ �� ���̸� ������
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPoint.transform.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"{enemy.name}���� {PlayerAttack.instance.playerDamage + damage}�� ���ظ� ����!");
        }
    }
}
