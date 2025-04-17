using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public int maxStamina;
    public int currentStamina;

    public float baseMoveSpeed;
    public float finalMoveSpeed;

    public float baseDamage;
    public float finalDamage;

    public float baseDefense;
    public float finalDefense;

    public void ModifyStat(BuffTargetStat stat, float value)
    {
        //if (stat == BuffTargetStat.Damage)
        //    finalDamage += value;
    }

    //public float GetFinalAttack() => finalDamage + modifiedAttack;

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log($"{gameObject.name}이(가) {amount}의 데미지를 입음. 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
