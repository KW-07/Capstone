using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchHit : MonoBehaviour
{
    public float damage;
    public int maxHitCount = 3;

    public int hitCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (hitCount >= maxHitCount)
            return;

        if (other.CompareTag("Enemy"))
        {
            // �� ĳ���Ͱ� ������ �޴� �޼��� ȣ�� (����)
            var enemyStats = other.GetComponent<CharacterStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(damage);
                Debug.Log($"�� {other.name}���� {damage} ������ ����");
            }

            hitCount++;

            if (hitCount >= maxHitCount)
            {
                Debug.Log("�ִ� Ÿ�� Ƚ�� ����, ȿ�� ����");
                Destroy(gameObject);
            }
        }
    }
}
