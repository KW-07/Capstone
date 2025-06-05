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
        Debug.Log($"{gameObject.name}에 {amount}의 데미지");
        if(currentHP < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} 사망");
    }
}
