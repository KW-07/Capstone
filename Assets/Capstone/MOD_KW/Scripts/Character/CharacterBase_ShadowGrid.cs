using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase_ShadowGrid : MonoBehaviour
{
    public float maxHP;
    protected float currentHP;

    protected Animator animator;
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currentHP = maxHP;
    }

    public virtual void TakeDamage(float amount)
    {
        currentHP -= amount;
        if(currentHP < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} »ç¸Á");
    }
}
