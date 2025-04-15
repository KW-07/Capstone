using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_Fist", menuName = "Player/Commands/F_Fist")]
public class F_FistCommandData : CommandData
{
    public float attackRange;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        Debug.Log($"{commandName}(F_Fist) 사용");

        GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
        Destroy(effect, destroyTime);

        //castPoint.GetComponent<MonoBehaviour>().StartCoroutine(DelayInstantiate(castPoint));

        // 생성위치 충돌 체크 및 적이면 데미지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPoint.transform.position, attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"{enemy.name}에게 {Player.instance.playerDamage + damage}의 피해를 입힘!");
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
