using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackEffect : MonoBehaviour
{
    private float knockbackForce;
    private float knockbackDuration;

    public void Setup(float force, float duration)
    {
        knockbackForce = force;
        knockbackDuration = duration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;

            rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

            StartCoroutine(ApplyKnockback(rb));
        }
    }

    private IEnumerator ApplyKnockback(Rigidbody2D rb)
    {
        float originalDrag = rb.drag;
        rb.drag = 5f;

        yield return new WaitForSeconds(knockbackDuration);

        rb.drag = originalDrag;
    }
}
