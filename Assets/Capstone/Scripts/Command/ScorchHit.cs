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
            // 적 캐릭터가 데미지 받는 메서드 호출 (예시)
            var enemyStats = other.GetComponent<CharacterStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(damage);
                Debug.Log($"적 {other.name}에게 {damage} 데미지 입힘");
            }

            hitCount++;

            if (hitCount >= maxHitCount)
            {
                Debug.Log("최대 타격 횟수 도달, 효과 종료");
                Destroy(gameObject);
            }
        }
    }
}
