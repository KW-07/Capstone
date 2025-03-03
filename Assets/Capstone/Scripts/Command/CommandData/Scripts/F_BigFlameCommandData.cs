using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "F_BigFlame", menuName = "Player/Commands/F_BigFlame")]
public class F_BigFlameCommandData : CommandData
{
    //public GameObject secondEffectPrefab;
    //public float secondDuration;

    public override void ActivateSkill(GameObject castPoint, GameObject target)
    {
        //Debug.Log($"{commandName}(F_BigFlame) 사용");

        //if (effectPrefab != null)
        //{
        //    // 첫번째 폭발범위 생성 및 삭제
        //    GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);
            
        //    // 두번쨰 장판 생성 및 삭제
        //    PlayerAttack.instance.DelayInstantiate(secondEffectPrefab, castPoint.transform.position, destroyTime);
        //    Destroy(effect, destroyTime);

        //    Destroy(secondEffectPrefab, destroyTime + secondDuration);
        //}

        //// 생성위치 충돌 체크 및 적이면 데미지
        //// 첫번째 폭발범위 내에 있으면 데미지
        //// 두번째 장판은 따로 오브젝트 생성
        //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPoint.transform.position, 0, LayerMask.GetMask("Enemy"));
        //if(effectPrefab.activeSelf)
        //{
        //    foreach (Collider2D enemy in hitEnemies)
        //    {
        //        Debug.Log($"{enemy.name}에게 {PlayerAttack.instance.playerDamage + damage}의 피해를 입힘!");
        //    }
        //}
    }
}
