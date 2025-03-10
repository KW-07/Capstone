using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile
{
    public float rotateSpeed = 200f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // 현재 회전 방향에서 목표 방향으로 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float newAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, rotateSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newAngle);

        // 앞으로 이동
        rb.velocity = transform.up * speed;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Debug.Log(collision.gameObject.tag);
            switch (collision.gameObject.tag)
            {
                case ("Ground"):
                case ("Platform"):
                    break;
                case ("Enemy"):
                    Destroy(gameObject);
                    break;
            }
        }
    }
}