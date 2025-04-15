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

        GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
        Destroy(effect, destroyTime);

        //castPoint.GetComponent<MonoBehaviour>().StartCoroutine(DelayInstantiate(castPoint));

        // ������ġ �浹 üũ �� ���̸� ������
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPoint.transform.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"{enemy.name}���� {Player.instance.playerDamage + damage}�� ���ظ� ����!");
        }

    }

    public override IEnumerator DelayInstantiate(GameObject castPoint)
    {
        yield return new WaitForSeconds(spawnDelay);

        GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);

        if(effect != null)
        {
            Destroy(effect, destroyTime);
        }
    }
}
