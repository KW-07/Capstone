using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CharacterBase_ShadowGrid : MonoBehaviour
{
    protected CharacterController controller;

    public float maxHP;
    protected float currentHP;

    protected bool isKnockbacked = false;

    public Animator animator;
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        currentHP = maxHP;
    }

    public virtual void TakeDamage(float amount)
    {
        currentHP -= amount;
        Debug.Log($"{gameObject.name}에 {amount}의 데미지");

        if(currentHP < 0)
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        if (isKnockbacked) return;
        StartCoroutine(KnockbackCoroutine(direction.normalized, force, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction, float force, float duration)
    {
        isKnockbacked = true;
        float timer = 0f;

        while (timer < duration)
        {
            controller.Move(direction * force * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isKnockbacked = false;
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} 사망");
    }
}
