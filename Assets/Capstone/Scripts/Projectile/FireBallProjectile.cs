using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // ���� �ð� �� �ڵ� �ı� (��: ȭ�� ������ ���ư� ���)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾�� �������� ��
            LivingEntity player = collision.GetComponent<LivingEntity>();
            if (player != null)
            {
                player.OnDamage(damage);
            }

            Destroy(gameObject); // �浹 �� �ı�
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // ���� ������ �ı� (���ϸ�)
            Destroy(gameObject);
        }
    }
}
