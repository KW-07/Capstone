using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackHandler : MonoBehaviour
{
    private Rigidbody rb;
    private bool isKnockedBack;
    private float knockbackDuration;
    private float knockbackTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
                rb.velocity = Vector3.zero; // ¸ØÃã Ã³¸®
            }
        }
    }

    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        isKnockedBack = true;
        knockbackDuration = duration;
        knockbackTimer = duration;

        rb.velocity = Vector3.zero; // ÃÊ±âÈ­
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }
}
