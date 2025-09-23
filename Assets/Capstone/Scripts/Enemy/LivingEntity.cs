using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public interface LivingEntity
{
    public void InitialSet();
    public void CheckHp();
    public void OnDamage(float damage);

}
/*    [Header("HP")]
    public Image currentHealthBar;
    public float maxHealth = 100f; //���� ü��
    public float currentHealth { get; protected set; } //���� ü��
    public bool dead { get; protected set; } //��� ����

    public event Action deathEvent; //��� �� �ߵ��� �̺�Ʈ

    //����ü�� Ȱ��ȭ�� �� ���¸� ����
    protected virtual void InitialSet()
    {
        //������� ���� ���·� ����
        dead = false;
        //ü���� ���� ü������ �ʱ�ȭ
        currentHealth = maxHealth;
    }
    public void CheckHp() //*HP ����
    {
        if (currentHealthBar != null)
            currentHealthBar.fillAmount = currentHealth / maxHealth;

        Debug.Log($"ü�¹� ���� fillAmount : {currentHealthBar.fillAmount}");
    }

    //���ظ� �޴� ���
    public virtual void OnDamage(float damage)
    {
        // �տ��� ���� ���� �� ü�¿��� ���� ������ �����ؾ� ��
        // ����) �ǰ� ������ = Ÿ�� ������(�μ��� �޾ƿ�) * ���� ����(?) * (1 - ����)
        //�ǰ� ��������ŭ ü�� ����

        currentHealth -= damage; // health = health - damage;
        CheckHp();
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
*/