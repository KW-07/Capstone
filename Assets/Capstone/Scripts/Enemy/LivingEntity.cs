using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class LivingEntity : MonoBehaviour
{
    [Header("HP")]
    public float maxHealth = 100f; //���� ü��
    public float currentHealth { get; protected set; } //���� ü��
    public bool dead { get; protected set; } //��� ����

    public event Action deathEvent; //��� �� �ߵ��� �̺�Ʈ

    //����ü�� Ȱ��ȭ�� �� ���¸� ����
    protected virtual void OnEnable()
    {
        //������� ���� ���·� ����
        dead = false;
        //ü���� ���� ü������ �ʱ�ȭ
        currentHealth = maxHealth;
    }

    //���ظ� �޴� ���
    public virtual void OnDamage(float damage)
    {
        //��������ŭ ü�� ����
        currentHealth -= damage; // health = health - damage;
        Debug.Log(this.gameObject.name + " take Damage.");

        //ü���� 0 ���� && ���� ���� �ʾҴٸ� ��� ó�� ����
        if (currentHealth <= 0 && !dead)
        {
            OnDie();
        }
    }

    //��� ó��
    public virtual void OnDie()
    {
        //onDeath �̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        if (deathEvent != null)
        {
            deathEvent();
        }

        dead = true;
        Debug.Log(this.gameObject.name + "is Die");
    }

}