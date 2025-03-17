using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//����ü�� ������ ���� ������Ʈ���� ���븦 ����
//ü��, ���ع���, ��� ���, ��� �̺�Ʈ ����

public class LivingEntity : MonoBehaviour//, IDamageable ���� ����
{
    public float maxHealth = 100f; //���� ü��
    public float maxStamina = 0f; //���� ����
    public float currentHealth { get; protected set; } //���� ü��
    public float currentStamina { get; protected set; } //���� ����
    public bool dead { get; protected set; } //��� ����

    public event Action onDeath; //��� �� �ߵ��� �̺�Ʈ

    //����ü�� Ȱ��ȭ�� �� ���¸� ����
    protected virtual void OnEnable()
    {
        //������� ���� ���·� ����
        dead = false;
        //ü���� ���� ü������ �ʱ�ȭ
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    //���ظ� �޴� ���
    public virtual void OnDamage(float damage)
    {
        //��������ŭ ü�� ����
        currentHealth -= damage; // health = health - damage;

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
        if (onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }

}