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
            Debug.Log($"{collision.name}���� {PlayerAttack.instance.playerDamage + damage}�� ���ظ� ����!");
            collision.GetComponent<LivingEntity>().OnDamage(PlayerAttack.instance.playerDamage + damage);

            // ������ ���� �� �ش� ������Ʈ ����
            Destroy(this.gameObject);
        }
    }
}
