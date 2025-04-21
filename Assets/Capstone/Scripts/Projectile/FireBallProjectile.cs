using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // 일정 시간 후 자동 파괴 (예: 화면 밖으로 날아간 경우)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어에게 데미지를 줌
            LivingEntity player = collision.GetComponent<LivingEntity>();
            if (player != null)
            {
                player.OnDamage(damage);
            }

            Destroy(gameObject); // 충돌 후 파괴
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // 땅에 닿으면 파괴 (원하면)
            Destroy(gameObject);
        }
    }
}
