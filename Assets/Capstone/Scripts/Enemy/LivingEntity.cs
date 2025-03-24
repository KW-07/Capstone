using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//생명체로 동작할 게임 오브젝트들의 뼈대를 제공
//체력, 피해받음, 사망 기능, 사망 이벤트 제공

public class LivingEntity : MonoBehaviour//, IDamageable 적용 예정
{
    public float maxHealth = 100f; //시작 체력
    public float maxStamina = 0f; //시작 마나
    public float currentHealth { get; protected set; } //현재 체력
    public float currentStamina { get; protected set; } //현재 마나
    public bool dead { get; protected set; } //사망 상태

    public event Action onDeath; //사망 시 발동할 이벤트

    //생명체가 활성화될 떄 상태를 리셋
    protected virtual void OnEnable()
    {
        //사망하지 않은 상태로 시작
        dead = false;
        //체력을 시작 체력으로 초기화
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    //피해를 받는 기능
    public virtual void OnDamage(float damage)
    {
        //데미지만큼 체력 감소
        currentHealth -= damage; // health = health - damage;

        //체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
        if (currentHealth <= 0 && !dead)
        {
            OnDie();
        }
    }

    //사망 처리
    public virtual void OnDie()
    {
        //onDeath 이벤트에 등록된 메서드가 있다면 실행
        if (onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }

}