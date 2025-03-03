using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_Dance", menuName = "Player/Commands/F_Dance")]
public class F_DanceCommandData : CommandData
{
    public float attackRange;
    public int repeatCount;
    public float repeatDelay;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName}(F_Dance) »ç¿ë");

        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
            Destroy(effect, destroyTime);
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPoint.transform.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            PlayerAttack.instance.RepeatAttack(enemy.gameObject, repeatCount, repeatDelay, damage);
        }
    }
}
