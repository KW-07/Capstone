using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCollider : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Debug.Log($"{collision.name}에게 {PlayerAttack.instance.playerDamage + damage}의 피해를 입힘!");
            collision.GetComponent<LivingEntity>().OnDamage(PlayerAttack.instance.playerDamage + damage);

            // 데미지 입힌 후 해당 오브젝트 삭제
            Destroy(this.gameObject);
        }
    }
}
