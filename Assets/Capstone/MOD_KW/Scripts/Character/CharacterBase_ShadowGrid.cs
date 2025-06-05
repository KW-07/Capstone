using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase_ShadowGrid : MonoBehaviour
{
    public float maxHP;
    protected float currentHP;

    public Animator animator;
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentHP = maxHP;
    }

    public virtual void TakeDamage(float amount)
    {
        currentHP -= amount;
        Debug.Log($"{gameObject.name}�� {amount}�� ������");
        if(currentHP < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} ���");
    }
}
